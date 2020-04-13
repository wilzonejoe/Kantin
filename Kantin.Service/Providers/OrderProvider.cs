using Core.Models.Auth;
using Core.Providers;
using Kantin.Data;
using Kantin.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Kantin.Service.Providers
{
    public class OrderProvider : GenericProvider<Order, KantinEntities>
    {
        public OrderProvider(KantinEntities context) : base(context) { }
        public OrderProvider(KantinEntities context, AccountIdentity accountIdentity) : base(context, accountIdentity) { }

        public override async Task<Order> Get(Guid id)
        {
            var order = await Context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.OrderAddOns)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                HandleItemNotFound(id);

            return order;
        }
    }
}
