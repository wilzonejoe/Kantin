using Core.Models.Abstracts;
using System;
using System.ComponentModel.DataAnnotations;

namespace Kantin.Data.Models
{
    public class MenuItemMenu : ValidationEntity
    {
        [Required]
        public Guid MenuItemId { get; set; }
        [Required]
        public Guid MenuId { get; set; }
        public Menu Menu { get; set; }
        public MenuItem MenuItem { get; set; }

        public MenuItemMenu() : base() { }
    }
}
