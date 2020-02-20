using Kantin.Data;
using Kantin.Data.Model;
using Kantin.Service.Exceptions;
using Kantin.Service.Interface;
using Kantin.Service.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kantin.Service
{
    public class MenuService : IService<MenuItem>, IDisposable
    {
        private KantinEntities _entities;

        public MenuService(KantinEntities entities)
        {
            _entities = entities;
        }

        public async Task<MenuItem> CreateAsync(MenuItem entity)
        {
            var result = await _entities.MenuItems.AddAsync(entity);
            await _entities.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            var menuItem = await GetMenuItemAsync(id);
            var result = _entities.MenuItems.Remove(menuItem);
            return result != null;
        }

        public async Task<MenuItem> Get(int id)
        {
            var menuItem = await _entities.MenuItems
                .Include(m => m.MenuAddOnItems)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (menuItem == null)
                throw new ItemNotFoundException();

            return menuItem;
        }

        public async Task<IEnumerable<MenuItem>> GetAll(Query query)
        {
            return await _entities.MenuItems.ToListAsync();
        }

        public async Task<MenuItem> UpdateAsync(int id, MenuItem entity)
        {
            var menuItem = await GetMenuItemAsync(id);
            _entities.Update<MenuItem>(entity);
            await _entities.SaveChangesAsync();
            return menuItem;
        }

        private async Task<MenuItem> GetMenuItemAsync(int id)
        {
            var menuItem = await _entities.MenuItems
                .FirstOrDefaultAsync(m => m.Id == id);

            if (menuItem == null) 
                throw new ItemNotFoundException();

            return menuItem;
        }

        public void Dispose()
        {
            _entities?.Dispose();
        }
    }
}
