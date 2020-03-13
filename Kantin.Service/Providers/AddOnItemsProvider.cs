using Core.Models.Auth;
using Core.Providers;
using Kantin.Data;
using Kantin.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Kantin.Service.Providers
{
    public class AddOnItemsProvider : GenericProvider<AddOnItem, KantinEntities>
    {
        public AddOnItemsProvider(KantinEntities context) : base(context) { }
        public AddOnItemsProvider(KantinEntities context, AccountIdentity accountIdentity) : base(context, accountIdentity) { }

        #region CRUD
        public override async Task<AddOnItem> Get(Guid id)
        {
            var addOnItem = await Context.AddOnItems
                .Include(a => a.MenuAddOnItems)
                .ThenInclude(m => m.MenuItem)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (addOnItem == null)
                HandleItemNotFound(id);

            return addOnItem;
        }
        #endregion
    }
}
