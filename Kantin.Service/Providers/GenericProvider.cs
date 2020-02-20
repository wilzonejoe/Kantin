using Kantin.Data;
using Kantin.Service.Exceptions;
using Kantin.Service.Interface;
using Kantin.Service.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kantin.Service.Providers
{
    public abstract class GenericProvider<T> : IService<T>, IDisposable
        where T : BaseEntity
    {
        private KantinEntities _context;
        protected KantinEntities Context => _context;

        public GenericProvider(KantinEntities context)
        {
            _context = context;
        }

        public abstract Task<T> Get(int id);
        public abstract Task<IEnumerable<T>> GetAll(Query query);

        public virtual async Task<T> CreateAsync(T entity)
        {
            var result = await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return (T)result.Entity;
        }

        public virtual async Task<bool> Delete(int id)
        {
            var item = await GetItemAsync(id);
            var result = _context.Remove(item);
            await _context.SaveChangesAsync();
            return result != null;
        }

        public virtual async Task<T> UpdateAsync(int id, T entity)
        {
            var item = await GetItemAsync(id);

            if (entity.Id != item.Id)
                entity.Id = item.Id;

            _context.Entry(item).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return await GetItemAsync(id);
        }

        private async Task<T> GetItemAsync(int id)
        {
            var item = await _context.FindAsync(typeof(T), id);

            if (item == null)
                throw new ItemNotFoundException();

            return (T)item;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
