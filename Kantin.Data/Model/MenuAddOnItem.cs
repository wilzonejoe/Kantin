using System;
using System.Collections.Generic;
using System.Text;

namespace Kantin.Data.Model
{
    public class MenuAddOnItem : IEntity
    {
        public int Id { get; set; }
        public int MenuItemId { get; set; }
        public int AddOnItemId { get; set; }
        public MenuItem MenutItem { get; set; }
        public AddOnItem AddOnItem { get; set; }
    }
}
