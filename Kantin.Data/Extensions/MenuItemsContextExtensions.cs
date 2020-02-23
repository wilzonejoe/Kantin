using Kantin.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Kantin.Data.Extensions
{
    public static class MenuItemsContextExtensions
    {
        public static void SetMenuItemsRelations(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuItem>()
                .HasMany(m => m.MenuAddOnItems)
                .WithOne(mad => mad.MenutItem);
        }
    }
}
