using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kantin.Data.Model
{
    public class AddOnItem : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public double Price { get; set; }
        public double Discount { get; set; }
        public double Available { get; set; }
        public virtual ICollection<MenuAddOnItem> MenuAddOnItems { get; set; }
    }
}
