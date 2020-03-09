using Kantin.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Kantin.Data.Extensions
{
    public static class AddOnItemsContextExtensions
    {
        public static void SetAddOnItemsRelations(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AddOnItem>()
                .HasMany(a => a.MenuAddOnItems)
                .WithOne(mad => mad.AddOnItem)
                .HasForeignKey(mad => mad.AddOnItemId)
                .HasConstraintName("FK_AddOnItem_MenuAddOnItem")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AddOnItem>()
               .HasOne(a => a.Organisation)
               .WithMany(o => o.AddOnItems);
        }
    }
}
