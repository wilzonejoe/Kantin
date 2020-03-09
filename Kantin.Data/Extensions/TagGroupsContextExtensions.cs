﻿using System;
using System.Collections.Generic;
using System.Text;
using Kantin.Data.Models.Tag;
using Microsoft.EntityFrameworkCore;

namespace Kantin.Data.Extensions
{
    public static class TagGroupsContextExtensions
    {
        public static void SetTagGroupsRelation(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TagGroup>()
                .HasMany(o => o.TagValues).WithOne();
        }
    }
}
