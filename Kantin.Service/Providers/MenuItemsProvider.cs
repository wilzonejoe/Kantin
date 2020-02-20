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
        private KantinEntities _entities;

        public MenuItemsProvider(KantinEntities entities) : base(entities)
        {
            _entities = entities;
        }

        public override async Task<MenuItem> Get(int id)
        {
            var menuItem = await _entities.MenuItems
                .Include(m => m.MenuAddOnItems)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (menuItem == null)
                throw new ItemNotFoundException();

            return menuItem;
        }

        public override async Task<IEnumerable<MenuItem>> GetAll(Query query)
        {
            return await _entities.MenuItems.ToListAsync();
        }
    }
}
