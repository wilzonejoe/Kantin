﻿using Core.Exceptions;
using Core.Providers;
using Kantin.Data;
using Kantin.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Kantin.Service.Providers
{
    public class MenuItemsProvider : GenericProvider<MenuItem, KantinEntities>
    {
        public MenuItemsProvider(KantinEntities context) : base(context) { }

        public override async Task<MenuItem> Get(int id)
        {
            var menuItem = await Context.MenuItems
                .Include(m => m.MenuAddOnItems)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (menuItem == null)
                throw new ItemNotFoundException();

            return menuItem;
        }
    }
}
