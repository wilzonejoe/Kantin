using Kantin.Data.Extensions;
using Kantin.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Kantin.Data
{
    public class KantinEntities: DbContext
    {
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<MenuAddOnItem> MenuAddOnItems { get; set; }
        public DbSet<AddOnItem> AddOnItems { get; set; }

        public KantinEntities(DbContextOptions<KantinEntities> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetPropertyAttribute(modelBuilder);
            SetRelation(modelBuilder);
        }

        private void SetPropertyAttribute(ModelBuilder modelBuilder)
        {
            modelBuilder.SetMenuItemsPropertyAttributes();
            modelBuilder.SetAddOnItemsPropertyAttributes();
            modelBuilder.SetMenuAddOnItemsPropertyAttributes();
        }

        private void SetRelation(ModelBuilder modelBuilder)
        {
            modelBuilder.SetMenuItemsRelations();
            modelBuilder.SetAddOnItemsRelations();
        }
    }
}
