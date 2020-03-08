using Core.Exceptions;
using Core.Providers;
using Kantin.Data;
using Kantin.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Kantin.Service.Providers
{
    public class MenuProvider : GenericProvider<Menu, KantinEntities>
    {
        public MenuProvider(KantinEntities context) : base(context) { }

        public override async Task<Menu> Get(int id)
        {
            var menu = await Context.Menus
                .Include(m => m.MenuItemsOnMenu)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (menu == null)
                throw new ItemNotFoundException();

            return menu;
        }
    }
}
