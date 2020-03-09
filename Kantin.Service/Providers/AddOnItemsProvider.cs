using Core.Providers;
using Kantin.Data;
using Kantin.Data.Models;
using Kantin.Service.Models.Auth;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Kantin.Service.Providers
{
    public class AddOnItemsProvider : GenericProvider<AddOnItem, KantinEntities>
    {
        public AccountIdentity AccountIdentity { get; private set; }
        public AddOnItemsProvider(KantinEntities context) : base(context) { }
        public AddOnItemsProvider(KantinEntities context, AccountIdentity accountIdentity) : base(context)
        {
            AccountIdentity = accountIdentity;
        }

        public override async Task<AddOnItem> Get(Guid id)
        {
            var addOnItem = await Context.AddOnItems
                .Include(m => m.MenuAddOnItems)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (addOnItem == null)
                HandleItemNotFound(id);

            return addOnItem;
        }

        public override Task<AddOnItem> Create(AddOnItem entity)
        {
            entity.OrganisationId = AccountIdentity.OrganisationId;
            return base.Create(entity);
        }

        public override Task<AddOnItem> Update(Guid id, AddOnItem entity)
        {
            entity.OrganisationId = AccountIdentity.OrganisationId;
            return base.Update(id, entity);
        }

        protected override async Task<AddOnItem> GetItem(Guid id)
        {
            var addOnItem = await base.GetItem(id);
            if (AccountIdentity == null)
                return addOnItem;

            if (addOnItem.OrganisationId != AccountIdentity.OrganisationId)
                HandleItemNotFound(id);

            return addOnItem;
        }
    }
}
