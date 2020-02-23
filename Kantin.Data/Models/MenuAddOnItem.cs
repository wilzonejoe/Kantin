using Kantin.Data.Models.Abstracts;

namespace Kantin.Data.Models
{
    public class MenuAddOnItem : ValidationEntity
    {
        public int MenuItemId { get; set; }
        public int AddOnItemId { get; set; }
        public MenuItem MenutItem { get; set; }
        public AddOnItem AddOnItem { get; set; }

        public MenuAddOnItem() : base() { }
    }
}
