using Medica_Interview.Infrastructure.Entities;
using Medica_Interview.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medica_Interview.Infrastructure.Repositories
{
    public class RecordRepository : IRepository<Employee>
    {
        private readonly IDataSourceReader _dataSource;

        public RecordRepository(IDataSourceReader dataSource)
        {
            _dataSource = dataSource;
        }

        public async Task<bool> DeleteData(Employee employee)
        {
            bool result = await _dataSource.DeleteDataAsync(employee);
            return result;
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            return await _dataSource.GetRecords();
        }

        public async Task<Employee> SaveData(Employee employee)
        {
            return await _dataSource.CreateDataAsync(employee);
        }

        public async Task<Employee> UpdateData(Employee employee)
        {
            return await _dataSource.UpdateDataAsync(employee);
        }
    }
}
