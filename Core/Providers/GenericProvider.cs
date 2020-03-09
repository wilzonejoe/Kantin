using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Core.Exceptions;
using Core.Interface;
using Core.Model;
using Core.Models.Abstracts;
using Microsoft.EntityFrameworkCore;
using Core.Helpers;

namespace Core.Providers
{
    public abstract class GenericProvider<T, R> : IService<T>, IDisposable
        where T : BaseEntity
        where R : DbContext
    {
        protected R Context { get; private set; }

        public GenericProvider(R context)
        {
            Context = context;
        }

        #region Basic CRUD
        public virtual async Task<IEnumerable<T>> GetAll(Query query)
        {
            return await Task.Run(() => Context.Set<T>().AsQueryable().ToList());
        }

        public virtual async Task<T> Get(Guid id)
        {
            return await GetItem(id);
        }

        public virtual async Task<T> Create(T entity)
        {
            ValidateEntity(entity);
            BeforeCreate(entity);

            if(entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();

            var result = await Context.AddAsync(entity);
            await Context.SaveChangesAsync();

            AfterCreate(entity);
            return (T)result.Entity;
        }

        public virtual async Task<bool> Delete(Guid id)
        {
            var item = await GetItem(id);
            BeforeDelete(item);

            var result = Context.Remove(item);
            await Context.SaveChangesAsync();

            AfterDelete(item);
            return result != null;
        }

        public virtual async Task<T> Update(Guid id, T entity)
        {
            var item = await ProcessEntityBeforeUpdate(id, entity);
            ValidateEntity(item);
            BeforeUpdate(item);

            Context.Entry(item).CurrentValues.SetValues(entity);
            await Context.SaveChangesAsync();

            AfterUpdate(item);
            return await GetItem(id);
        }
        #endregion

        #region handlers
        protected virtual async Task<T> GetItem(Guid id)
        {
            var item = await Context.FindAsync(typeof(T), id);

            if (item == null)
                HandleItemNotFound(id);

            return (T)item;
        }

        protected void HandleItemNotFound(Guid id)
        {
            string errorMessage;
            var typeName = typeof(T).Name;

            if (id == Guid.Empty)
            {
                errorMessage = $"{typeName} Id not valid";
            }
            else
            {
                var errorTemplate = "Item {0} with id {1} is not found";
                errorMessage = string.Format(errorTemplate, typeName, id);
            }

            throw new ItemNotFoundException(errorMessage);
        }

        protected void ValidateEntity(T entity)
        {
            if (entity is ValidationEntity validationEntity)
                validationEntity.Validate();
        }

        protected async Task<T> ProcessEntityBeforeUpdate(Guid id, T entity)
        {
            var item = await GetItem(id);

            if (entity.Id != item.Id)
                entity.Id = item.Id;

            return item;
        }
        #endregion

        #region before action handlers
        protected void BeforeCreate(T entity) { }
        protected void BeforeUpdate(T entity) { }
        protected void BeforeDelete(T entity) { }
        #endregion

        #region after action handlers
        protected void AfterCreate(T entity) { }
        protected void AfterUpdate(T entity) { }
        protected void AfterDelete(T entity) { }
        #endregion

        #region Dispose
        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
                Context?.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
