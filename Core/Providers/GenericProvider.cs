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
using Core.Models.Auth;
using Core.Models.Interfaces;
using Core.Extensions;

namespace Core.Providers
{
    public abstract class GenericProvider<T, R> : IService<T>, IDisposable
        where T : BaseEntity
        where R : DbContext
    {
        protected AccountIdentity AccountIdentity { get; private set; }
        protected R Context { get; private set; }

        public GenericProvider(R context)
        {
            Context = context;
        }

        public GenericProvider(R context, AccountIdentity accountIdentity)
        {
            Context = context;
            AccountIdentity = accountIdentity;
        }

        #region Basic CRUD
        public virtual async Task<IEnumerable<T>> GetAll(Query query)
        {
            return await Task.Run(() => Context.Set<T>().AsQueryable().ToList());
        }

        public virtual async Task<T> Get(Guid id)
        {
            return await GetItem(id, false);
        }

        public virtual async Task<T> Create(T entity)
        {
            ValidateEntity(entity);

            if(entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();

            if (entity is IOrganisationModel organisationModel && organisationModel.OrganisationId == Guid.Empty)
                organisationModel.SetOrganisationId(AccountIdentity?.OrganisationId);

            await BeforeCreate(entity);

            var result = await Context.AddAsync(entity);
            await Context.SaveChangesAsync();

            await AfterCreate(entity);
            return result.Entity as T;
        }

        public virtual async Task<bool> Delete(Guid id)
        {
            var item = await GetItem(id);
            await BeforeDelete(item);

            var result = Context.Remove(item);
            await Context.SaveChangesAsync();

            await AfterDelete(item);
            return result != null;
        }

        public virtual async Task<T> Update(Guid id, T entity)
        {
            var item = await ProcessEntityBeforeUpdate(id, entity);
            ValidateEntity(item);
            await BeforeUpdate(item);

            if (entity is IOrganisationModel organisationModel)
                organisationModel.SetOrganisationId(AccountIdentity?.OrganisationId);

            Context.Entry(item).CurrentValues.SetValues(entity);
            await Context.SaveChangesAsync();

            await AfterUpdate(item);
            return await GetItem(id);
        }
        #endregion

        #region handlers
        protected virtual async Task<T> GetItem(Guid id, bool checkOrganisation = true)
        {
            var item = await Context.FindAsync(typeof(T), id);

            if (item == null)
                HandleItemNotFound(id);

            if (checkOrganisation && item is IOrganisationModel model && 
                model.OrganisationId != AccountIdentity.OrganisationId)
                HandleItemNotFound(id);

            return item as T;
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
        #pragma warning disable 1998
        protected virtual async Task BeforeCreate(T entity) { }
        protected virtual async Task BeforeUpdate(T entity) { }
        protected virtual async Task BeforeDelete(T entity) { }
        #pragma warning restore 1998
        #endregion

        #region after action handlers
        #pragma warning disable 1998
        protected virtual async Task AfterCreate(T entity) { }
        protected virtual async Task AfterUpdate(T entity) { }
        protected virtual async Task AfterDelete(T entity) { }
        #pragma warning restore 1998
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
