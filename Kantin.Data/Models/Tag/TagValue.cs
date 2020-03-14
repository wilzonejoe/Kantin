using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Core.Models.Abstracts;

namespace Kantin.Data.Models.Tag
{
    public class TagValue : ValidationEntity
    {
        public Guid ItemId { get; set; }
        public ItemType ItemType { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        public Guid TagGroupId { get; set; }

        public TagGroup TagGroup { get; set; }
        public Guid OrganisationId { get; set; }
        public Organisation Organisation { get; set; }
        public string Subtitle { get; set; }
    }
}
