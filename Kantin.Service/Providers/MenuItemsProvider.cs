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
                .Include(m => m.MenuItemAttachments)
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
            await ProcessMenuItemMenus(entity, true);
            await base.BeforeCreate(entity);
        }

        protected override async Task BeforeUpdate(MenuItem entity)
        {
            await ProcessMenuAddOnItem(entity, false);
            await ProcessMenuItemMenus(entity, false);
            await base.BeforeUpdate(entity);
        }

        protected override Task BeforeDelete(MenuItem entity)
        {
            ClearMenuAddOnItems(entity.Id);
            ClearMenuItemMenus(entity.Id);
            ClearMenuItemAttachments(entity.Id);
            return base.BeforeDelete(entity);
        }

        private void ClearMenuAddOnItems(Guid menuItemId)
        {
            var existedMenuAddOnItems= Context.MenuAddOnItems.Where(mad => mad.MenuItemId == menuItemId);
            Context.MenuAddOnItems.RemoveRange(existedMenuAddOnItems);
        }

        private void ClearMenuItemMenus(Guid menuItemId)
        {
            var existedMenuItemMenus = Context.MenuItemMenus.Where(miom => miom.MenuItemId == menuItemId);
            Context.MenuItemMenus.RemoveRange(existedMenuItemMenus);
        }
        private void ClearMenuItemAttachments(Guid menuItemId)
        {
            var existedMenuItemAttachments = Context.MenuItemAttachments.Where(mia => mia.MenuItemId == menuItemId);
            Context.MenuItemAttachments.RemoveRange(existedMenuItemAttachments);
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

        private async Task ProcessMenuItemMenus(MenuItem item, bool isNew)
        {
            var MenuItemMenus = item.MenuItemMenus;

            if (!isNew)
                ClearMenuItemMenus(item.Id);

            if (MenuItemMenus != null && MenuItemMenus.Any())
            {
                var invalidMenuItemMenus = MenuItemMenus.Where(mad => mad.MenuId == Guid.Empty);
                foreach (var invalidMenuItemMenu in invalidMenuItemMenus)
                    MenuItemMenus.Remove(invalidMenuItemMenu);

                foreach (var menuItemMenu in MenuItemMenus)
                {
                    if (menuItemMenu.Id == Guid.Empty)

                        if (menuItemMenu.Id == Guid.Empty)
                            menuItemMenu.Id = Guid.NewGuid();

                    var menuExisted = Context.Menus.Any(a => a.Id == menuItemMenu.MenuId);

                    if (menuExisted)
                        continue;

                    menuItemMenu.MenuItemId = item.Id;
                    await Context.MenuItemMenus.AddAsync(menuItemMenu);
                }

                item.MenuAddOnItems.Clear();
            }
        }

    }
}
