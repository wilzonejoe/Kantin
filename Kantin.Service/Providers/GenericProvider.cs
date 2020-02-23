using Kantin.Data;
using Kantin.Data.Models.Abstracts;
using Kantin.Data.Exceptions;
using Kantin.Service.Interface;
using Kantin.Service.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Kantin.Service.Providers
{
    public abstract class GenericProvider<T> : IService<T>, IDisposable
        where T : BaseEntity
    {
        protected KantinEntities Context { get; private set; }

        public GenericProvider(KantinEntities context)
        {
            Context = context;
        }

        public virtual async Task<IEnumerable<T>> GetAll(Query query)
        {
            return await Task<T>.Run(() => Context.Set<T>().AsQueryable().ToList());
        }

        public virtual async Task<T> Get(int id)
        {
            return await GetItemAsync(id);
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            ValidateEntity(entity);
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
            var item = await ProcessEntityBeforeUpdate(id, entity);
            ValidateEntity(item);
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

        protected void ValidateEntity(T entity)
        {
            if (entity is ValidationEntity validationEntity)
                validationEntity.Validate();
        }

        protected async Task<T> ProcessEntityBeforeUpdate(int id, T entity)
        {
            var item = await GetItemAsync(id);

            if (entity.Id != item.Id)
                entity.Id = item.Id;

            return item;
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}
