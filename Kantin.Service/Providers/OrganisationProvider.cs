using Core.Exceptions;
using Core.Exceptions.Models;
using Core.Models.Auth;
using Core.Providers;
using Kantin.Data;
using Kantin.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kantin.Service.Providers
{
    public class OrganisationProvider : GenericProvider<Organisation, KantinEntities>
    {
        public OrganisationProvider(KantinEntities context) : base(context) { }
        public OrganisationProvider(KantinEntities context, AccountIdentity accountIdentity) : base(context, accountIdentity) { }

        public override async Task<Organisation> Get(Guid id)
        {
            var organisation = await base.GetItem(id, false);

            if (AccountIdentity.OrganisationId == null || organisation.Id != AccountIdentity.OrganisationId)
                return organisation;

            return Context.Organisations
                .Include(o => o.Accounts)
                .FirstOrDefault(o => o.Id == id);
        }

        protected override async Task BeforeCreate(Organisation entity)
        {
            var account = Context.Accounts.FirstOrDefault(a => a.Id == AccountIdentity.AccountId);
            CheckOrganisationCreateOrUpdateEligibility(entity, account);

            entity.Id = Guid.NewGuid();
            entity.ExpiryDateUTC = DateTime.UtcNow.AddDays(SystemConstants.TrialPeriod);

            account.OrganisationId = entity.Id;
            await base.BeforeCreate(entity);
        }

        protected override Task AfterCreate(Organisation entity)
        {
            SetAccountAsOrganisationAdmin(entity.Id);
            return base.AfterCreate(entity);
        }

        protected override async Task BeforeUpdate(Organisation entity)
        {
            var account = Context.Accounts.FirstOrDefault(a => a.Id == AccountIdentity.AccountId);
            CheckOrganisationCreateOrUpdateEligibility(entity, account);
            await base.BeforeUpdate(entity);
        }

        protected override async Task BeforeDelete(Organisation entity)
        {
            if (entity.ExpiryDateUTC <= DateTime.UtcNow)
                return;

            var accounts = Context.Accounts.Where(a => a.OrganisationId == entity.Id);
            await accounts.ForEachAsync(a => a.OrganisationId = null);
            await RemoveAllRelatedEntities(entity);
            await base.BeforeDelete(entity);
        }

        private void CheckOrganisationCreateOrUpdateEligibility(Organisation organisation, Account account)
        {
            var propertyErrors = new List<PropertyErrorResult>();

            if (account.OrganisationId != null)
            {
                propertyErrors.Add(new PropertyErrorResult
                {
                    FieldErrors = $"Account has an organisation already"
                });
            }
            else
            {
                var errorMessageTemplate = "{0} with value of {1} has been taken";
                var organisationExisted = Context.Organisations.Any(a => a.Name.ToLower() == organisation.Name.ToLower());

                if (!organisationExisted)
                    return;

                propertyErrors.Add(new PropertyErrorResult
                {
                    FieldName = nameof(organisation.Name),
                    FieldErrors = string.Format(errorMessageTemplate, nameof(Organisation.Name), organisation.Name)
                });
            }

            throw new ConflictException(propertyErrors);
        }

        private void SetAccountAsOrganisationAdmin(Guid organisationId)
        {
            if (!AccountIdentity.AccountId.HasValue)
                throw new UnauthorizedException("Provide a valid access token");

            var privilege = new Privilege()
            {
                Id = Guid.NewGuid(),
                AccountId = AccountIdentity.AccountId.Value,
                OrganisationId = organisationId,
                Name = "Admin",
                CanAccessMenu = true,
                CanAccessOrder = true,
                CanAccessStaffMember = true,
                CanAccessSettings = true
            };

            Context.Privileges.Add(privilege);
            Context.SaveChanges();
        }

        private async Task RemoveAllRelatedEntities(Organisation organisation)
        {
            var menuItemsToDelete = Context.MenuItems
                .Include(mi => mi.MenuAddOnItems)
                .Include(mi => mi.MenuItemMenus)
                .Include(mi => mi.MenuItemAttachments)
                .Where(mi => mi.OrganisationId == organisation.Id);

            var menuAddOnItemsToDelete = new List<MenuAddOnItem>();
            await menuItemsToDelete.Select(mi => mi.MenuAddOnItems).ForEachAsync(mads =>
            {
                foreach (var mad in mads)
                    menuAddOnItemsToDelete.Add(mad);
            });

            var menuItemMenusToDelete = new List<MenuItemMenu>();
            await menuItemsToDelete.Select(mi => mi.MenuItemMenus).ForEachAsync(mioms =>
            {
                foreach (var miom in mioms)
                    menuItemMenusToDelete.Add(miom);
            });

            var menuItemAttachmentsToDelete = new List<MenuItemAttachment>();
            await menuItemsToDelete.Select(mi => mi.MenuItemAttachments).ForEachAsync(mias =>
            {
                foreach (var mia in mias)
                    menuItemAttachmentsToDelete.Add(mia);
            });

            var addOnItemsToDelete = Context.AddOnItems
                .Include(a => a.AddOnItemAttachments)
                .Where(a => a.OrganisationId == organisation.Id);

            var addOnItemAttachmentsToDelete = new List<AddOnItemAttachment>();
            await addOnItemsToDelete.Select(a => a.AddOnItemAttachments).ForEachAsync(aoias =>
            {
                foreach (var aoia in aoias)
                    addOnItemAttachmentsToDelete.Add(aoia);
            });

            var menusToDelete = Context.Menus.Where(m => m.OrganisationId == organisation.Id);

            Context.MenuAddOnItems.RemoveRange(menuAddOnItemsToDelete);
            Context.MenuItemAttachments.RemoveRange(menuItemAttachmentsToDelete);
            Context.MenuItemMenus.RemoveRange(menuItemMenusToDelete);
            Context.AddOnItemAttachments.RemoveRange(addOnItemAttachmentsToDelete);
            Context.AddOnItems.RemoveRange(addOnItemsToDelete);
            Context.MenuItems.RemoveRange(menuItemsToDelete);
            Context.Menus.RemoveRange(menusToDelete);
        }
    }
}
