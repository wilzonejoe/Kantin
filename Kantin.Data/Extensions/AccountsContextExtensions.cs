using Core.Models.Auth;
using Kantin.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Kantin.Data.Extensions
{
    public static class AccountsContextExtensions
    {
        public static void SetAccountsRelations(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasMany(a => a.Sessions)
                .WithOne(s => s.Account);
        }
    }
}
