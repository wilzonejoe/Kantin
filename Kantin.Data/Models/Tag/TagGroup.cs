using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Core.Models.Abstracts;

namespace Kantin.Data.Models.Tag
{
    public class TagGroup : ValidationEntity
    {
        [Key]
        [MaxLength(50)]
        public string Title { get; set; }

        public virtual ICollection<TagValue> TagValues { get; set; }
    }
}
