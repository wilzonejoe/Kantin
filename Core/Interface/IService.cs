using Core.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interface
{
    public interface IService<T> where T : class
    {
        public Task<T> Create(T entity);
        public Task<bool> Delete(Guid id);
        public Task<T> Get(Guid id);
        public Task<IEnumerable<T>> GetAll(Query query);
        public Task<T> Update(Guid id, T entity);
    }
}
