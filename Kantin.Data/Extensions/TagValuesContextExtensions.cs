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
                .HasKey(t => new { t.ItemId, t.ItemType, t.Title });

            modelBuilder.Entity<TagValue>()
                .HasOne(o => o.TagGroup)
                .WithMany()
                .HasForeignKey(t => t.TagGroupId);
        }
    }
}
