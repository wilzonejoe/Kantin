using Core.Models.Abstracts;
using System;
using System.ComponentModel.DataAnnotations;

namespace Kantin.Data.Models
{
    public class MenuAddOnItem : ValidationEntity
    {
        [Required]
        public Guid MenuItemId { get; set; }

        [Required]
        public Guid AddOnItemId { get; set; }

        public MenuItem MenutItem { get; set; }
        public AddOnItem AddOnItem { get; set; }

        public MenuAddOnItem() : base() { }
    }
}
