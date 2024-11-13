using CsvHelper;
using Medica_Interview.Infrastructure.Entities;
using Medica_Interview.Infrastructure.Enum;
using Medica_Interview.Infrastructure.Interface;
using Medica_Interview.Infrastructure.ServiceModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Medica_Interview.Infrastructure.Repositories
{
    public class CsvFileRepo : IDataSourceReader
    {
        private readonly ILogger<CsvFileRepo> _logger;
        private readonly IConfiguration _configuration;
        private readonly string source = "";
        public CsvFileRepo(ILogger<CsvFileRepo> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            source = _configuration.GetConnectionString("CsvConnection");
        }


        public async Task<IEnumerable<Employee>> GetRecords()
        {
            try
            {
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

                    return employees;
                }
            }
            catch (Exception ex) {
                return null;
            }
        }

        // Async Create operation
        public async Task<Employee> CreateDataAsync(Employee employee)
        {
            var employees = (await GetRecords()).ToList();
            if (employees.Where(x => x.Email == employee.Email).FirstOrDefault() == null)
            {
                employees.Add(employee);

                await WriteRecordsAsync((IEnumerable<Employee>)employees, RecordOperation.Save);

                return employee;
            }
            Employee employee1 = new Employee();
            return employee1;
        }

        // Async Update operation
        public async Task<Employee> UpdateDataAsync(Employee employee)
        {
            Employee employee1 = new Employee();
            var employees = (await GetRecords()).ToList();
            var index = employees.FindIndex(r => r.Email == employee.Email);

            if (index != -1)
            {
                employees[index] = employee;
                await WriteRecordsAsync((IEnumerable<Employee>)employees, RecordOperation.Update);
                employee1 = employee;
            }
            else
            {
                _logger.LogWarning("Record with ID {Id} not found, update skipped.", employee.Email);
            }

            return employee1;
        }

        // Async Delete operation
        public async Task<bool> DeleteDataAsync(Employee employee)
        {
            var employees = (await GetRecords()).ToList();
            var employeeToRemove = employees.FirstOrDefault(r => r.Email == employee.Email);

            if (employeeToRemove != null)
            {
                employees.Remove(employeeToRemove);
                await WriteRecordsAsync(employees, RecordOperation.Delete);
                return true;
            }
            else
            {
                _logger.LogWarning("Record with ID {Id} not found, delete skipped.", employee.Email);
                return false;
            }
        }

        private async Task WriteRecordsAsync(IEnumerable<Employee> employees, RecordOperation operation)
        {
            _logger.LogInformation("Writing records asynchronously to CSV file at path");
            bool fileExists = File.Exists(source) && new FileInfo(source).Length > 0;

            List<Employee> existingEmployees = new List<Employee>();
            if (fileExists)
            {
                using (var reader = new StreamReader(source))
                using (var csv = new CsvReader(reader, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = true }))
                {
                    csv.Context.RegisterClassMap<EmployeeMap>();
                    existingEmployees = csv.GetRecords<Employee>().ToList();
                }
            }


            string[] headers = { "Firstname", "Lastname", "Email","Telephone", "Date of Birth", "Address 1",
                            "Address 2","Town","County","Postcode","Job Title","Team","Line Manager","Start Date","Profile Picture" };
            switch (operation)
            {
                case RecordOperation.Save:
                    // Filter out any employees that already exist in the file based on a unique identifier (e.g., Email)
                    var newEmployees = employees.Where(e => !existingEmployees.Any(existing => existing.Email == e.Email)).ToList();
                    if (!newEmployees.Any())
                    {
                        _logger.LogInformation("No new records to save; all provided records are duplicates.");
                        return;
                    }

                    var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HasHeaderRecord = !fileExists // Only write headers if the file is new
                    };
                    using (var writer = new StreamWriter(source, append: true))
                    using (var csv = new CsvWriter(writer, config))
                    {
                        await csv.WriteRecordsAsync(newEmployees);
                    }
                    break;

                case RecordOperation.Update:
                    // Update existing records based on a unique identifier (e.g., Email)
                    foreach (var employee in employees)
                    {
                        var index = existingEmployees.FindIndex(e => e.Email == employee.Email);
                        if (index != -1)
                        {
                            existingEmployees[index] = employee; // Replace with updated record
                        }
                    }

                    var configUpdate = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HasHeaderRecord = false
                    };

                    
                    using (var writer = new StreamWriter(source))
                    using (var csv = new CsvWriter(writer, configUpdate))
                    {
                        
                        // Write custom headers
                        foreach (var header in headers)
                        {
                            await writer.WriteAsync(header + ",");
                        }
                        await writer.WriteLineAsync();
                        await csv.WriteRecordsAsync(existingEmployees); // Rewrite all records after updating
                    }
                    break;

                case RecordOperation.Delete:
                    // Delete records based on a unique identifier (e.g., Email)
                   
                    var configDelete = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HasHeaderRecord = false
                    };

                    using (var writer = new StreamWriter(source))
                    using (var csv = new CsvWriter(writer, configDelete))
                    {
                        // Write custom headers
                        foreach (var header in headers)
                        {
                            await writer.WriteAsync(header + ",");
                        }
                        await writer.WriteLineAsync();

                        await csv.WriteRecordsAsync(employees); // Rewrite all records after deleting
                    }
                    break;

                default:
                    throw new ArgumentException("Invalid operation specified.", nameof(operation));
            }

            _logger.LogInformation("New records have been written to the CSV file.");
        }
    }
}
