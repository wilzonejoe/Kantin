namespace Kantin.Data.Model
{
    public class MenuAddOnItem : BaseEntity
    {
        public int MenuItemId { get; set; }
        public int AddOnItemId { get; set; }
        public MenuItem MenutItem { get; set; }
        public AddOnItem AddOnItem { get; set; }
    }
}
