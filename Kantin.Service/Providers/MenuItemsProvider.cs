using Core.Providers;
using Kantin.Data;
using Kantin.Data.Models;
using Kantin.Service.Models.Auth;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Kantin.Service.Providers
{
    public class MenuItemsProvider : GenericProvider<MenuItem, KantinEntities>
    {
        public AccountIdentity AccountIdentity { get; private set; }
        public MenuItemsProvider(KantinEntities context) : base(context) { }
        public MenuItemsProvider(KantinEntities context, AccountIdentity accountIdentity) : base(context) 
        {
            AccountIdentity = accountIdentity;
        }

        public override async Task<MenuItem> Get(Guid id)
        {
            var menuItem = await Context.MenuItems
                .Include(m => m.MenuAddOnItems)
                .ThenInclude(m => m.AddOnItem)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (menuItem == null)
                HandleItemNotFound(id);

            return menuItem;
        }

        public override Task<MenuItem> Create(MenuItem entity)
        {
            entity.OrganisationId = AccountIdentity.OrganisationId;
            return base.Create(entity);
        }

        public override Task<MenuItem> Update(Guid id, MenuItem entity)
        {
            entity.OrganisationId = AccountIdentity.OrganisationId;
            return base.Update(id, entity);
        }

        protected override async Task<MenuItem> GetItem(Guid id)
        {
            var menuItem = await base.GetItem(id);
            if (AccountIdentity == null)
                return menuItem;

            if (menuItem.OrganisationId != AccountIdentity.OrganisationId)
                HandleItemNotFound(id);

            return menuItem;
        }
    }
}
