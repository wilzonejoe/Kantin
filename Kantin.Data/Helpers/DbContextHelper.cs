using Core.Models.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;

namespace Kantin.Data.Helpers
{
    public static class DbContextHelper
    {
        public static void SetOperationDate(ChangeTracker changeTracker)
        {
            var entries = changeTracker.Entries()
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


        public static void RestrictDeletion(ModelBuilder modelBuilder)
        {
            // Set restriction to restrict on the deletion. 
            // Due to default ondelete behaviour is Cascade means that it will automagically deleting all of the entities related to the entity

            var foreignKeys = modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys());

            foreach (var foreignKey in foreignKeys)
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}
