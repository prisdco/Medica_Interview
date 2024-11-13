using Medica_Interview.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medica_Interview.Infrastructure.Interface
{
    public interface IDataSourceReader
    {
        Task<IEnumerable<Employee>> GetRecords();
        Task<Employee> CreateDataAsync(Employee employee);
        Task<bool> DeleteDataAsync(Employee employee);
        Task<Employee> UpdateDataAsync(Employee employee);
    }
}
