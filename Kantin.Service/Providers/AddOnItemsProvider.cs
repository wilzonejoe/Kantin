using Kantin.Data;
using Kantin.Data.Model;
using Kantin.Service.Exceptions;
using Kantin.Service.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kantin.Service.Providers
{
    public class AddOnItemsProvider : GenericProvider<AddOnItem>
    {
        private KantinEntities _entities;

        public AddOnItemsProvider(KantinEntities entities) : base(entities)
        {
            _entities = entities;
        }

        public override async Task<AddOnItem> Get(int id)
        {
            var addOnItem = await _entities.AddOnItems
                .Include(m => m.MenuAddOnItems)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (addOnItem == null)
                throw new ItemNotFoundException();

            return addOnItem;
        }

        public override async Task<IEnumerable<AddOnItem>> GetAll(Query query)
        {
            return await _entities.AddOnItems.ToListAsync();
        }
    }
}
