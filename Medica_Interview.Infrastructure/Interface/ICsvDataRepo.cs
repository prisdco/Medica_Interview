using Medica_Interview.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Medica_Interview.Infrastructure.Interface
{
    public interface ICsvDataRepo
    {
        Task<Employee> CreateCsvDataAsync(Employee employee);
        Task<bool> DeleteCsvDataAsync(Employee employee);
        Task<Employee> UpdateCsvDataAsync(Employee employee);
    }
}
