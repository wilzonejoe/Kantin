using Core.Exceptions;
using Core.Providers;
using Kantin.Data;
using Kantin.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Kantin.Service.Providers
{
    public class AddOnItemsProvider : GenericProvider<AddOnItem, KantinEntities>
    {
        public AddOnItemsProvider(KantinEntities context) : base(context) { }

        public override async Task<AddOnItem> Get(int id)
        {
            var addOnItem = await Context.AddOnItems
                .Include(m => m.MenuAddOnItems)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (addOnItem == null)
                throw new ItemNotFoundException();

            return addOnItem;
        }
    }
}
