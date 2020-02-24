﻿using Core;
using Kantin.Data.Extensions;
using Kantin.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Kantin.Data
{
    public class KantinEntities : DbContext
    {
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<MenuAddOnItem> MenuAddOnItems { get; set; }
        public DbSet<AddOnItem> AddOnItems { get; set; }

        public KantinEntities(DbContextOptions<DbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetRelation(modelBuilder);
        }

        private void SetRelation(ModelBuilder modelBuilder)
        {
            modelBuilder.SetMenuItemsRelations();
            modelBuilder.SetAddOnItemsRelations();
        }
    }
}
