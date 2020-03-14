﻿using Kantin.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Kantin.Data.Extensions
{
    public static class TagGroupsContextExtensions
    {
        public static void SetTagGroupsRelation(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TagGroup>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<TagGroup>()
                .HasMany(o => o.TagValues).WithOne();

            modelBuilder.Entity<TagGroup>()
                .HasOne(m => m.Organisation)
                .WithMany(o => o.TagGroups);
        }
    }
}
