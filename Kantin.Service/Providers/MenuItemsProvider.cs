using Kantin.Data;
using Kantin.Data.Model;
using Kantin.Service.Exceptions;
using Kantin.Service.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kantin.Service.Providers
{
    public class MenuItemsProvider : GenericProvider<MenuItem>
    {
        public MenuItemsProvider(KantinEntities context) : base(context)
        {
        }

        public override async Task<MenuItem> Get(int id)
        {
            var menuItem = await Context.MenuItems
                .Include(m => m.MenuAddOnItems)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (menuItem == null)
                throw new ItemNotFoundException();

            return menuItem;
        }
    }
}
