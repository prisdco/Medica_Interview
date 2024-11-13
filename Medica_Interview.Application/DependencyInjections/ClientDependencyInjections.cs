using MediatR;
using Medica_Interview.Application.Extensions;
using Medica_Interview.Application.UseCase.Queries.CsvFileData;
using Medica_Interview.Infrastructure;
using Medica_Interview.Infrastructure.Entities;
using Medica_Interview.Infrastructure.Interface;
using Medica_Interview.Infrastructure.Repositories;
using Medica_Interview.Infrastructure.ServiceModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Medica_Interview.Application.DependencyInjections
{
    public static class ClientDependencyInjections
    {
        public static IServiceCollection AddApplicationClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IDataSourceReader>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<CsvFileRepo>>();
                var dataSourceType = configuration["DataSource"];
                return dataSourceType switch
                {
                    "Database" => new DatabaseRepo(provider.GetRequiredService<ApplicationDbContext>()),
                    "Csv" => new CsvFileRepo(logger, configuration),
                    _ => throw new InvalidOperationException("Invalid data source configuration")
                };
            });
            services.AddScoped<IRepository<Employee>, RecordRepository>();
            // Register new extension data source type to services
            services.AddDatabaseSourceType<DatabaseRepo>();
            services.AddMediatR(typeof(AllEmployeesQuery.AllEmployeesQueryHandler).GetTypeInfo().Assembly);

            services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer("YourConnectionStringHere"));

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.MetadataPropertyHandling = MetadataPropertyHandling.Ignore;
                options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.Converters.Add(new StringEnumConverter());

            }).ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errorsInModelState = context.ModelState
                        .Where(x => x.Value.Errors.Count > 0)
                        .ToDictionary(pair => pair.Key,
                            pair => pair.Value.Errors.Select(x => x.ErrorMessage))
                        .ToArray();

                    var errorViewModel = new ErrorViewModel();

                    //cycle through the errors and add to response
                    foreach (var (key, value) in errorsInModelState)
                    {
                        foreach (var subError in value)
                        {
                            errorViewModel.Errors.Add(new Error
                            {
                                Code = key,
                                Message = subError
                            });
                        }
                    }

                    var error = ResultViewModel.Fail(
                        errorViewModel,
                        $" {StatusCodes.Status400BadRequest.ToString()} : Error occured"
                    );

                    return new BadRequestObjectResult(error);

                };
            });

            #region swagger implementation
            services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation    
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Medica's - WebApi",
                    Description = "Medica's Developer web api portal for third party consumption"
                });


            });
            #endregion


            return services;
        }
    }

}
