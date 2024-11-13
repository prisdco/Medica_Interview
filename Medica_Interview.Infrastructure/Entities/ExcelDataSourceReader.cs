using CsvHelper;
using Medica_Interview.Infrastructure.ServiceModel;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Medica_Interview.Infrastructure.Entities
{
    public class ExcelDataSourceReader : SourceReaderProcessor
    {
        public override async Task<string> ProcessDataSource()
        {
            var data = new List<DataSourceReader>();
            var _configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json")
           .Build();
            string source = _configuration.GetValue<string>("csvfileSetting:Path");
            using (var reader = new StreamReader(source))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<EmployeeMap>();

                // Read records asynchronously
                var employees = new List<Employee>();
                await foreach (var record in csv.GetRecordsAsync<Employee>())
                {
                    employees.Add(record);
                }

                // Convert the list of employees to a JSON string asynchronously
                return Newtonsoft.Json.JsonConvert.SerializeObject(employees, Formatting.Indented);
            }
        }
    }
}
    
