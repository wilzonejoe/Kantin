using Kantin.Service.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kantin.Service.Interface
{
    public interface IService<T> where T : class
    {
        public Task<T> CreateAsync(T entity);
        public Task<bool> Delete(int id);
        public Task<T> Get(int id);
        public Task<IEnumerable<T>> GetAll(Query query);
        public Task<T> UpdateAsync(int id, T entity);
    }
}
