using Kantin.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Kantin.Data.Extensions
{
    public static class MenuAddOnItemsContextExtensions
    {
        public static void SetMenuAddOnItemsPropertyAttributes(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuAddOnItem>()
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
