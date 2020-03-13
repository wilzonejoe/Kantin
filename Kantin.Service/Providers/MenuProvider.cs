using Core.Models.Auth;
using Core.Providers;
using Kantin.Data;
using Kantin.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Kantin.Service.Providers
{
    public class MenuProvider : GenericProvider<Menu, KantinEntities>
    {
        public MenuProvider(KantinEntities context) : base(context) { }
        public MenuProvider(KantinEntities context, AccountIdentity accountIdentity) : base(context, accountIdentity) { }

        public override async Task<Menu> Get(Guid id)
        {
            var menu = await Context.Menus
                .Include(m => m.MenuItemsOnMenu)
                .ThenInclude(m => m.MenuItem)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (menu == null)
                HandleItemNotFound(id);

            return menu;
        }
    }
}
