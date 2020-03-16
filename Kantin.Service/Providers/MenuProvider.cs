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
                .Include(m => m.MenuItemsOnMenus)
                .ThenInclude(m => m.MenuItem)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (menu == null)
                HandleItemNotFound(id);

            return menu;
        }

        protected override async Task BeforeCreate(Menu entity)
        {
            await ProcessMenuItemOnMenus(entity, true);
            await base.BeforeCreate(entity);
        }

        protected override async Task BeforeUpdate(Menu entity)
        {
            await ProcessMenuItemOnMenus(entity, false);
            await base.BeforeUpdate(entity);
        }

        protected override Task BeforeDelete(Menu entity)
        {
            ClearMenuItemsOnMenus(entity.Id);
            return base.BeforeDelete(entity);
        }

        private void ClearMenuItemsOnMenus(Guid menuId)
        {
            var existedMenuItemOnMenus = Context.MenuItemsOnMenus.Where(miom => miom.MenuId == menuId);
            Context.MenuItemsOnMenus.RemoveRange(existedMenuItemOnMenus);
        }

        private async Task ProcessMenuItemOnMenus(Menu item, bool isNew)
        {
            var menuItemOnMenus = item.MenuItemsOnMenus;

            if (!isNew)
                ClearMenuItemsOnMenus(item.Id);

            if (menuItemOnMenus != null && menuItemOnMenus.Any())
            {
                var invalidMenuItemOnMenus = menuItemOnMenus.Where(mad => mad.MenuItemId == Guid.Empty);
                foreach (var invalidMenuItemOnMenu in invalidMenuItemOnMenus)
                    menuItemOnMenus.Remove(invalidMenuItemOnMenu);

                foreach (var menuItemOnMenu in menuItemOnMenus)
                {
                    if (menuItemOnMenu.Id == Guid.Empty)

                        if (menuItemOnMenu.Id == Guid.Empty)
                            menuItemOnMenu.Id = Guid.NewGuid();

                    var menuExisted = Context.MenuItems.Any(a => a.Id == menuItemOnMenu.MenuItemId);

                    if (menuExisted)
                        continue;

                    menuItemOnMenu.MenuId = item.Id;
                    await Context.MenuItemsOnMenus.AddAsync(menuItemOnMenu);
                }

                item.MenuItemsOnMenus.Clear();
            }
        }

        public IQueryable<Menu> Paging(in int pageNumber, in int pageSize)
        {
            var menus = Context.Menus.OrderBy(a => a.Title);
            return menus.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }
}
