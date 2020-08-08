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
    public class MenuProvider : GenericProvider<Menu, KantinEntities>
    {
        public MenuProvider(KantinEntities context) : base(context) { }
        public MenuProvider(KantinEntities context, AccountIdentity accountIdentity) : base(context, accountIdentity) { }

        public override async Task<Menu> Get(Guid id)
        {
            var menu = await Context.Menus
                .Include(m => m.MenuItemMenus)
                .ThenInclude(m => m.MenuItem)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (menu == null)
                HandleItemNotFound(id);

            return menu;
        }

        protected override async Task BeforeCreate(Menu entity)
        {
            await ProcessMenuItemMenus(entity, true);
            await base.BeforeCreate(entity);
        }

        protected override async Task BeforeUpdate(Menu entity)
        {
            await ProcessMenuItemMenus(entity, false);
            await base.BeforeUpdate(entity);
        }

        protected override Task BeforeDelete(Menu entity)
        {
            ClearMenuItemMenus(entity.Id);
            return base.BeforeDelete(entity);
        }

        private void ClearMenuItemMenus(Guid menuId)
        {
            var existedMenuItemMenus = Context.MenuItemMenus.Where(miom => miom.MenuId == menuId);
            Context.MenuItemMenus.RemoveRange(existedMenuItemMenus);
        }

        private async Task ProcessMenuItemMenus(Menu item, bool isNew)
        {
            var menuItemMenus = item.MenuItemMenus;

            if (!isNew)
                ClearMenuItemMenus(item.Id);

            if (menuItemMenus != null && menuItemMenus.Any())
            {
                var invalidMenuItemMenus = menuItemMenus.Where(mad => mad.MenuItemId == Guid.Empty);
                foreach (var invalidMenuItemMenu in invalidMenuItemMenus)
                    menuItemMenus.Remove(invalidMenuItemMenu);

                foreach (var menuItemMenu in menuItemMenus)
                {
                    if (menuItemMenu.Id == Guid.Empty)

                        if (menuItemMenu.Id == Guid.Empty)
                            menuItemMenu.Id = Guid.NewGuid();

                    var menuExisted = Context.MenuItems.Any(a => a.Id == menuItemMenu.MenuItemId);

                    if (menuExisted)
                        continue;

                    menuItemMenu.MenuId = item.Id;
                    await Context.MenuItemMenus.AddAsync(menuItemMenu);
                }

                item.MenuItemMenus.Clear();
            }
        }
    }
}
