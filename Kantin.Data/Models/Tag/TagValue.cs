using System;
using System.ComponentModel.DataAnnotations;
using Core.Models.Abstracts;

namespace Kantin.Data.Models
{
    public class TagValue : ValidationEntity
    {
        public Guid ItemId { get; set; }
        public TagItemType ItemType { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        public Guid TagGroupId { get; set; }

        public TagGroup TagGroup { get; set; }
        public Guid OrganisationId { get; set; }
        public Organisation Organisation { get; set; }
        public string Subtitle { get; set; }
    }
}
