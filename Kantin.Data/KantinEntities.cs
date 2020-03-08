using Core.Models.Abstracts;
using Core.Models.Auth;
using Kantin.Data.Extensions;
using Kantin.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kantin.Data
{
    public class KantinEntities : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<BaseSession> Sessions { get; set; }
        public DbSet<Organisation> Organisations { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<MenuAddOnItem> MenuAddOnItems { get; set; }
        public DbSet<AddOnItem> AddOnItems { get; set; }

        public KantinEntities(DbContextOptions<KantinEntities> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetRelation(modelBuilder);
        }

        private void SetRelation(ModelBuilder modelBuilder)
        {
            modelBuilder.SetAccountsRelations();
            modelBuilder.SetOrganisationRelations();
            modelBuilder.SetMenuItemsRelations();
            modelBuilder.SetAddOnItemsRelations();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            SetOperationDate();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            SetOperationDate();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetOperationDate();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            SetOperationDate();
            return base.SaveChanges();
        }

        private void SetOperationDate()
        {
            var entries = ChangeTracker.Entries()
                .Where(x => x.Entity is BaseEntity);

            foreach (var entry in entries)
            {
                entry.Property(nameof(BaseEntity.UpdatedDateUTC)).CurrentValue = DateTime.UtcNow;

                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.Property(nameof(BaseEntity.CreatedDateUTC)).IsModified = false;
                        break;
                    case EntityState.Added:
                        entry.Property(nameof(BaseEntity.CreatedDateUTC)).CurrentValue = DateTime.UtcNow;
                        break;
                }
            }
        }
    }
}
