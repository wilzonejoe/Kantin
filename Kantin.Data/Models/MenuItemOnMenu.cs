using Core.Models.Abstracts;

namespace Kantin.Data.Models
{
    public class MenuItemOnMenu : ValidationEntity
    {
        public int MenuItemId { get; set; }
        public int MenuId { get; set; }
        public Menu Menu { get; set; }
        public MenuItem MenuItem { get; set; }

        public MenuItemOnMenu() : base() { }
    }
}
