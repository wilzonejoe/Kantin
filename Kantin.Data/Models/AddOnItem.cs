using Core.Models.Abstracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kantin.Data.Models
{
    public class AddOnItem : ValidationEntity
    {
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public double Price { get; set; }
        public double Discount { get; set; }
        public bool Available { get; set; }
        public Guid OrganisationId { get; set; }

        [JsonIgnore]
        public Organisation Organisation { get; set; }

        [JsonIgnore]
        public virtual ICollection<MenuAddOnItem> MenuAddOnItems { get; set; }

        public AddOnItem() : base()
        {
            Available = true;
            MenuAddOnItems = new List<MenuAddOnItem>();
        }
    }
}
