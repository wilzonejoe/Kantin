using Kantin.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Kantin.Data.Extensions
{
    public static class MenuAddOnItemsContextExtensions
    {
        public static void SetMenuAddOnItemsRelations(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuAddOnItem>()
                .HasOne(m => m.MenutItem);

            modelBuilder.Entity<MenuAddOnItem>()
                .HasOne(m => m.AddOnItem);
        }
    }
}
