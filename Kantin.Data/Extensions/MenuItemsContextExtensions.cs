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
                .WithOne(mad => mad.MenuItem)
                .HasForeignKey(mad => mad.MenuItemId)
                .HasConstraintName("FK_MenuItem_MenuAddOnItem")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MenuItem>()
                .HasMany(m => m.MenuItemsOnMenus)
                .WithOne(miom => miom.MenuItem)
                .HasForeignKey(miom => miom.MenuItemId)
                .HasConstraintName("FK_MenuItem_MenuItemsOnMenu")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MenuItem>()
                .HasOne(m => m.Organisation)
                .WithMany(o => o.MenuItems);
        }
    }
}
