using Core.Exceptions;
using Core.Providers;
using Kantin.Data;
using Kantin.Data.Models;
using Kantin.Service.Models.Auth;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Kantin.Service.Providers
{
    public class MenuProvider : GenericProvider<Menu, KantinEntities>
    {
        public AccountIdentity AccountIdentity { get; private set; }
        public MenuProvider(KantinEntities context) : base(context) { }
        public MenuProvider(KantinEntities context, AccountIdentity accountIdentity) : base(context)
        {
            AccountIdentity = accountIdentity;
        }

        public override async Task<Menu> Get(Guid id)
        {
            var menu = await Context.Menus
                .Include(m => m.MenuItemsOnMenu)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (menu == null)
                HandleItemNotFound(id);

            return menu;
        }

        public override Task<Menu> Create(Menu entity)
        {
            entity.OrganisationId = AccountIdentity.OrganisationId;
            return base.Create(entity);
        }

        public override Task<Menu> Update(Guid id, Menu entity)
        {
            entity.OrganisationId = AccountIdentity.OrganisationId;
            return base.Update(id, entity);
        }

        protected override async Task<Menu> GetItem(Guid id)
        {
            var menu = await base.GetItem(id);
            if (AccountIdentity == null)
                return menu;

            if (menu.OrganisationId != AccountIdentity.OrganisationId)
                HandleItemNotFound(id);

            return menu;
        }
    }
}
