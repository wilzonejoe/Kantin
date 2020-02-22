using Kantin.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Kantin.Data.Extensions
{
    public static class AddOnItemsContextExtensions
    {
        public static void SetAddOnItemsRelations(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AddOnItem>()
                .HasMany(a => a.MenuAddOnItems)
                .WithOne(mad => mad.AddOnItem);
        }
    }
}
