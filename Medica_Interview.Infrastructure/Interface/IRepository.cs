using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medica_Interview.Infrastructure.Interface
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> SaveData(T entity);
        Task<T> UpdateData(T entity);
        Task<bool> DeleteData(T entity);
    }
}
