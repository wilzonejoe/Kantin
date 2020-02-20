using System;
using System.Collections.Generic;
using System.Text;

namespace Kantin.Data.Model
{
    public class MenuItem : IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public double Available { get; set; }

        public virtual ICollection<MenuAddOnItem> MenuAddOnItems { get; set; }
    }
}
