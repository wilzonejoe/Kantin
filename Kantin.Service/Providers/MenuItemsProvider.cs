using Core.Models.Auth;
using Core.Providers;
using Kantin.Data;
using Kantin.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kantin.Service.Providers
{
    public class MenuItemsProvider : GenericProvider<MenuItem, KantinEntities>
    {
        public MenuItemsProvider(KantinEntities context) : base(context) { }
        public MenuItemsProvider(KantinEntities context, AccountIdentity accountIdentity) : base(context, accountIdentity) { }

        public override async Task<MenuItem> Get(Guid id)
        {
            var menuItem = await Context.MenuItems
                .Include(m => m.MenuAddOnItems)
                .ThenInclude(m => m.AddOnItem)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (menuItem == null)
                HandleItemNotFound(id);

            return menuItem;
        }

        protected override async Task BeforeCreate(MenuItem entity)
        {
            await ProcessMenuAddOnItem(entity, true);
            await ProcessMenuItemOnMenus(entity, true);
            await base.BeforeCreate(entity);
        }

        protected override async Task BeforeUpdate(MenuItem entity)
        {
            await ProcessMenuAddOnItem(entity, false);
            await ProcessMenuItemOnMenus(entity, false);
            await base.BeforeUpdate(entity);
        }

        protected override Task BeforeDelete(MenuItem entity)
        {
            ClearMenuAddOnItems(entity.Id);
            ClearMenuItemsOnMenus(entity.Id);
            return base.BeforeDelete(entity);
        }

        private void ClearMenuAddOnItems(Guid menuItemId)
        {
            var existedMenuAddOnItems= Context.MenuAddOnItems.Where(mad => mad.MenuItemId == menuItemId);
            Context.MenuAddOnItems.RemoveRange(existedMenuAddOnItems);
        }

        private void ClearMenuItemsOnMenus(Guid menuItemId)
        {
            var existedMenuItemOnMenus = Context.MenuItemsOnMenus.Where(miom => miom.MenuItemId == menuItemId);
            Context.MenuItemsOnMenus.RemoveRange(existedMenuItemOnMenus);
        }

        private async Task ProcessMenuAddOnItem(MenuItem item, bool isNew)
        {
            var menuAddOnItems = item.MenuAddOnItems;

            if (!isNew)
                ClearMenuAddOnItems(item.Id);

            if (menuAddOnItems != null && menuAddOnItems.Any())
            {
                var invalidMenuAddOnItems = menuAddOnItems.Where(mad => mad.AddOnItemId == Guid.Empty);
                foreach (var invalidMenuAddOnItem in invalidMenuAddOnItems)
                    menuAddOnItems.Remove(invalidMenuAddOnItem);

                foreach (var menuAddOnItem in menuAddOnItems)
                {
                    if (menuAddOnItem.Id == Guid.Empty)

                        if (menuAddOnItem.Id == Guid.Empty)
                            menuAddOnItem.Id = Guid.NewGuid();

                    var addOnItemExisted = Context.AddOnItems.Any(a => a.Id == menuAddOnItem.AddOnItemId);

                    if (addOnItemExisted)
                        continue;

                    menuAddOnItem.MenuItemId = item.Id;
                    await Context.MenuAddOnItems.AddAsync(menuAddOnItem);
                }

                item.MenuAddOnItems.Clear();
            }
        }

        private async Task ProcessMenuItemOnMenus(MenuItem item, bool isNew)
        {
            var menuItemOnMenus = item.MenuItemsOnMenus;

            if (!isNew)
                ClearMenuItemsOnMenus(item.Id);

            if (menuItemOnMenus != null && menuItemOnMenus.Any())
            {
                var invalidMenuItemOnMenus = menuItemOnMenus.Where(mad => mad.MenuId == Guid.Empty);
                foreach (var invalidMenuItemOnMenu in invalidMenuItemOnMenus)
                    menuItemOnMenus.Remove(invalidMenuItemOnMenu);

                foreach (var menuItemOnMenu in menuItemOnMenus)
                {
                    if (menuItemOnMenu.Id == Guid.Empty)

                        if (menuItemOnMenu.Id == Guid.Empty)
                            menuItemOnMenu.Id = Guid.NewGuid();

                    var menuExisted = Context.Menus.Any(a => a.Id == menuItemOnMenu.MenuId);

                    if (menuExisted)
                        continue;

                    menuItemOnMenu.MenuItemId = item.Id;
                    await Context.MenuItemsOnMenus.AddAsync(menuItemOnMenu);
                }

                item.MenuAddOnItems.Clear();
            }
        }

    }
}
