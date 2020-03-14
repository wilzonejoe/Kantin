using System;
using System.Collections.Generic;
using System.Text;
using Kantin.Data.Models;
using Kantin.Data.Models.Tag;
using Microsoft.EntityFrameworkCore;

namespace Kantin.Data.Extensions
{
    public static class TagValuesContextExtensions
    {
        public static void SetTagValuesRelation(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TagValue>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<TagValue>()
                .HasOne(s => s.TagGroup)
                .WithMany(g => g.TagValues)
                .HasForeignKey(s => s.TagGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TagValue>()
                .HasOne(m => m.Organisation)
                .WithMany(o => o.TagValues);

        }
    }
}
