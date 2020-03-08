using Core.Models.Abstracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kantin.Data.Models
{
    public class Menu : ValidationEntity
    {
        [Required]
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public bool Available { get; set; }
        public virtual ICollection<MenuItemOnMenu> MenuItemsOnMenu { get; set; }

        public Menu() : base()
        {
            MenuItemsOnMenu = new List<MenuItemOnMenu>();
            Available = true;
        }
    }
}
