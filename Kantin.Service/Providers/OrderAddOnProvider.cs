using Core.Models.Auth;
using Core.Providers;
using Kantin.Data;
using Kantin.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Kantin.Service.Providers
{
    public class OrderAddOnProvider : GenericProvider<OrderAddOn, KantinEntities>
    {
        public OrderAddOnProvider(KantinEntities context) : base(context) { }
        public OrderAddOnProvider(KantinEntities context, AccountIdentity accountIdentity) : base(context, accountIdentity) { }

        public override async Task<OrderAddOn> Get(Guid id)
        {
            var orderAddOn = await Context.OrderAddOns
                .FirstOrDefaultAsync(o => o.Id == id);

            if (orderAddOn == null)
                HandleItemNotFound(id);

            return orderAddOn;
        }
    }
}
