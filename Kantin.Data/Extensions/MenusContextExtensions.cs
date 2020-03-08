using Kantin.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Kantin.Data.Extensions
{
    public static class MenusContextExtensions
    {
        public static void SetMenusRelations(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Menu>()
                .HasMany(a => a.MenuItemsOnMenu)
                .WithOne(mad => mad.Menu);
        }
    }
}
