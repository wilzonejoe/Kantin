using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Models.Abstracts;

namespace Kantin.Data.Models
{
    public class TagGroup : ValidationEntity
    {
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        public Guid OrganisationId { get; set; }
        public Organisation Organisation { get; set; }
        public virtual ICollection<TagValue> TagValues { get; set; }

        public TagGroup() : base()
        {
            TagValues = new List<TagValue>();
        }
    }
}
