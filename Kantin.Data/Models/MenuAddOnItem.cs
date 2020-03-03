using Core.Models.Abstracts;

namespace Kantin.Data.Models
{
    public class MenuAddOnItem : ValidationEntity
    {
        public int MenuItemId { get; set; }
        public int AddOnItemId { get; set; }
        public MenuItem MenuItem { get; set; }
        public AddOnItem AddOnItem { get; set; }

        public MenuAddOnItem() : base() { }
    }
}
