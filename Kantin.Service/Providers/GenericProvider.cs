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
        private KantinEntities _entities;

        public GenericProvider(KantinEntities entities)
        {
            _entities = entities;
        }

        public abstract Task<T> Get(int id);
        public abstract Task<IEnumerable<T>> GetAll(Query query);

        public virtual async Task<T> CreateAsync(T entity)
        {
            var result = await _entities.AddAsync(entity);
            await _entities.SaveChangesAsync();
            return (T)result.Entity;
        }

        public virtual async Task<bool> Delete(int id)
        {
            var item = await GetItemAsync(id);
            var result = _entities.Remove(item);
            await _entities.SaveChangesAsync();
            return result != null;
        }

        public virtual async Task<T> UpdateAsync(int id, T entity)
        {
            var item = await GetItemAsync(id);

            if (entity.Id != item.Id)
                entity.Id = item.Id;

            _entities.Entry(item).CurrentValues.SetValues(entity);
            await _entities.SaveChangesAsync();
            return await GetItemAsync(id);
        }

        private async Task<T> GetItemAsync(int id)
        {
            var item = await _entities.FindAsync(typeof(T), id);

            if (item == null)
                throw new ItemNotFoundException();

            return (T)item;
        }

        public void Dispose()
        {
            _entities?.Dispose();
        }
    }
}
