using System.Collections.Generic;

namespace Kantin.Data.Model
{
    public class AddOnItem : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public double Available { get; set; }
        public virtual ICollection<MenuAddOnItem> MenuAddOnItems { get; set; }
    }
}
