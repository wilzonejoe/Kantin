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
        where T : class
    {
        protected KantinEntities Context { get; private set; }

        public GenericProvider(KantinEntities context)
        {
            Context = context;
        }

        public virtual async Task<IEnumerable<T>> GetAll(Query query)
        {
            var queryable = Context.Set<T>().AsQueryable();
            return new List<T>(queryable);
        }

        public virtual async Task<T> Get(int id)
        {
            return await GetItemAsync(id);
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            var result = await Context.AddAsync(entity);
            await Context.SaveChangesAsync();
            return (T)result.Entity;
        }

        public virtual async Task<bool> Delete(int id)
        {
            var item = await GetItemAsync(id);
            var result = Context.Remove(item);
            await Context.SaveChangesAsync();
            return result != null;
        }

        public virtual async Task<T> UpdateAsync(int id, T entity)
        {
            var item = await GetItemAsync(id);

            if (item is BaseEntity exitingItem && entity is BaseEntity baseEntity)
            {
                if (baseEntity.Id != exitingItem.Id)
                    baseEntity.Id = exitingItem.Id;
            }

            Context.Entry(item).CurrentValues.SetValues(entity);
            await Context.SaveChangesAsync();
            return await GetItemAsync(id);
        }

        private async Task<T> GetItemAsync(int id)
        {
            var item = await Context.FindAsync(typeof(T), id);

            if (item == null)
                throw new ItemNotFoundException();

            return (T)item;
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}
