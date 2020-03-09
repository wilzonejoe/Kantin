using Kantin.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Kantin.Data.Extensions
{
    public static class OrganisationsContextExtensions
    {
        public static void SetOrganisationRelations(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Organisation>()
                .HasMany(o => o.Accounts)
                .WithOne(a => a.Organisation);
        }
    }
}
