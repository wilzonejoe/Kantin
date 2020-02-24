using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Core.Exceptions;
using Core.Interface;
using Core.Model;
using Core.Models.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace Core.Providers
{
    public abstract class GenericProvider<T,R> : IService<T>, IDisposable
        where T : BaseEntity
        where R : DbContext
    {
        protected R Context { get; private set; }

        public GenericProvider(R context)
        {
            Context = context;
        }

        public virtual async Task<IEnumerable<T>> GetAll(Query query)
        {
            return await Task.Run(() => Context.Set<T>().AsQueryable().ToList());
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
