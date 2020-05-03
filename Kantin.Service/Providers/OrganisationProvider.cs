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
            CheckOrganisationCreateOrUpdateEligibility(entity);

            entity.Id = Guid.NewGuid();
            entity.ExpiryDateUTC = DateTime.UtcNow.AddDays(SystemConstants.TrialPeriod);
            SetAccountAsOrganisationAdmin(entity.Id);
            await base.BeforeCreate(entity);
        }

        protected override async Task BeforeUpdate(Organisation entity)
        {
            CheckOrganisationCreateOrUpdateEligibility(entity);
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

        private void CheckOrganisationCreateOrUpdateEligibility(Organisation organisation)
        {
            var propertyErrors = new List<PropertyErrorResult>();

            var account = Context.Accounts.FirstOrDefault(a => a.Id == AccountIdentity.AccountId);

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
                CanAccessMenu = true,
                CanAccessOrder = true,
                CanAccessStaffMember = true,
                CanAccessSettings = true
            };

            Context.Privileges.Add(privilege);
        }

        private async Task RemoveAllRelatedEntities(Organisation organisation)
        {
            var menuItemsToDelete = Context.MenuItems
                .Include(mi => mi.MenuAddOnItems)
                .Include(mi => mi.MenuItemsOnMenus)
                .Where(mi => mi.OrganisationId == organisation.Id);

            var menuAddOnItemsToDelete = new List<MenuAddOnItem>();
            await menuItemsToDelete.Select(mi => mi.MenuAddOnItems).ForEachAsync(mads =>
            {
                foreach (var mad in mads)
                    menuAddOnItemsToDelete.Add(mad);
            });

            var menuItemsOnMenusToDelete = new List<MenuItemOnMenu>();
            await menuItemsToDelete.Select(mi => mi.MenuItemsOnMenus).ForEachAsync(mioms =>
            {
                foreach (var miom in mioms)
                    menuItemsOnMenusToDelete.Add(miom);
            });

            var addOnItemsToDelete = Context.AddOnItems.Where(a => a.OrganisationId == organisation.Id);
            var menusToDelete = Context.Menus.Where(m => m.OrganisationId == organisation.Id);

            Context.MenuAddOnItems.RemoveRange(menuAddOnItemsToDelete);
            Context.MenuItemsOnMenus.RemoveRange(menuItemsOnMenusToDelete);
            Context.AddOnItems.RemoveRange(addOnItemsToDelete);
            Context.MenuItems.RemoveRange(menuItemsToDelete);
            Context.Menus.RemoveRange(menusToDelete);
        }
    }
}
