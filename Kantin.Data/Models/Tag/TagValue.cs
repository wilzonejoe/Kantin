using System;
using System.ComponentModel.DataAnnotations;
using Core.Models.Abstracts;
using Kantin.Data.Enumerable;
using Newtonsoft.Json;

namespace Kantin.Data.Models
{
    public class TagValue : ValidationEntity
    {
        public Guid ItemId { get; set; }
        public TagItemType ItemType { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public Guid TagGroupId { get; set; }

        public TagGroup TagGroup { get; set; }

        [JsonIgnore]
        public Guid OrganisationId { get; set; }

        [JsonIgnore]
        public Organisation Organisation { get; set; }
    }
}
