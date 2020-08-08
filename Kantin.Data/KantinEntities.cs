using Kantin.Data.Helpers;
using Kantin.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Kantin.Data
{
    public class KantinEntities : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Organisation> Organisations { get; set; }
        public DbSet<Privilege> Privileges { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuItemMenu> MenuItemMenus { get; set; }
        public DbSet<MenuItemAttachment> MenuItemAttachments { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<MenuAddOnItem> MenuAddOnItems { get; set; }
        public DbSet<AddOnItemAttachment> AddOnItemAttachments { get; set; }
        public DbSet<AddOnItem> AddOnItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderAddOn> OrderAddOns { get; set; }
        public DbSet<TagValue> TagValues { get; set; }
        public DbSet<TagGroup> TagGroups { get; set; }

        public KantinEntities(DbContextOptions<KantinEntities> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DbContextHelper.RestrictDeletion(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            DbContextHelper.SetOperationDate(ChangeTracker);
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            DbContextHelper.SetOperationDate(ChangeTracker);
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            DbContextHelper.SetOperationDate(ChangeTracker);
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            DbContextHelper.SetOperationDate(ChangeTracker);
            return base.SaveChanges();
        }
    }
}
