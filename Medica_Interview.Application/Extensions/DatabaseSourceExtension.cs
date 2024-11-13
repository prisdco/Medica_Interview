using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medica_Interview.Application.Extensions
{

    public static class DatabaseSourceExtension
    {
        //Adding a new class of datasource
        //Check the ServiceModel for database as source as an example
        public static void AddDatabaseSourceType<T>(this IServiceCollection services) where T : class
        {
            // For example, registering it in a factory
            services.AddScoped(typeof(T));
        }
    }
}
