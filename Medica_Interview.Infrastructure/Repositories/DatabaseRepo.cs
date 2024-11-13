using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medica_Interview.Infrastructure.Entities;
using Medica_Interview.Infrastructure.Interface;

namespace Medica_Interview.Infrastructure.Repositories
{

    public class DatabaseRepo : IDataSourceReader
    {
        private readonly ApplicationDbContext _context;

        public DatabaseRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<Employee> CreateDataAsync(Employee employee)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteDataAsync(Employee employee)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> UpdateDataAsync(Employee employee)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Employee>> GetRecords()
        {
            throw new NotImplementedException();
        }
    }
}
