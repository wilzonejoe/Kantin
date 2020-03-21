﻿using Kantin.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Kantin.Data.Extensions
{
    public static class TagValuesContextExtensions
    {
        public static void SetTagValuesRelation(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TagValue>()
                .HasOne(s => s.TagGroup)
                .WithMany(g => g.TagValues)
                .HasForeignKey(s => s.TagGroupId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
