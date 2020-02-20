using Kantin.Data.Model;
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

        public static void SetMenuItemsPropertyAttributes(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuItem>()
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
