using Core.Models.Auth;
using Core.Providers;
using Kantin.Data;
using Kantin.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Kantin.Service.Providers
{
    public class OrderItemProvider : GenericProvider<OrderItem, KantinEntities>
    {
        public OrderItemProvider(KantinEntities context) : base(context) { }
        public OrderItemProvider(KantinEntities context, AccountIdentity accountIdentity) : base(context, accountIdentity) { }

        public override async Task<OrderItem> Get(Guid id)
        {
            var orderItem = await Context.OrderItems
                .Include(oi => oi.OrderAddOns)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (orderItem == null)
                HandleItemNotFound(id);

            return orderItem;
        }
    }
}
