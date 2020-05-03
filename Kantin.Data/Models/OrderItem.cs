using System;
using Core.Models.Abstracts;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Kantin.Data.Models
{
    public class OrderItem : ValidationEntity
    {
        [Required]
        public Guid OrderId { get; set; }
        [Required]
        public Guid MenuItemId { get; set; }
        [JsonIgnore]
        public Order Order { get; set; }
        [JsonIgnore]
        public MenuItem MenuItem { get; set; }
        public string MenuItemName { get; set; }
        public double MenuItemPrice { get; set; }
        public int MenuItemAmount { get; set; }
        public double MenuItemTotal { get; set; }
        [JsonIgnore]
        public ICollection<OrderAddOn> OrderAddOns { get; set; }

        public OrderItem() : base()
        {
            MenuItemName = MenuItem.Title;
            MenuItemPrice = MenuItem.Price;
            MenuItemAmount = 1;
            OrderAddOns = new List<OrderAddOn>();
        }

        public void CalculateMenuItemTotal()
        {
            MenuItemTotal = (MenuItemAmount * MenuItemPrice) + OrderAddOns.Sum(orderAddOn => orderAddOn.AddOnItemTotal);
        }
    }
}
