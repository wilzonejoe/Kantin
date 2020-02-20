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
        public AddOnItemsProvider(KantinEntities context) : base(context)
        {
        }

        public override async Task<AddOnItem> Get(int id)
        {
            var addOnItem = await Context.AddOnItems
                .Include(m => m.MenuAddOnItems)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (addOnItem == null)
                throw new ItemNotFoundException();

            return addOnItem;
        }

        public override async Task<IEnumerable<AddOnItem>> GetAll(Query query)
        {
            return await Context.AddOnItems.ToListAsync();
        }
    }
}
