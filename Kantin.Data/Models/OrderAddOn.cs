using System;
using System.ComponentModel.DataAnnotations;
using Core.Models.Abstracts;
using Newtonsoft.Json;

namespace Kantin.Data.Models
{
    public class OrderAddOn : ValidationEntity
    {
        [Required]
        public Guid OrderItemId { get; set; }
        [Required]
        public Guid AddOnItemId { get; set; }
        public string AddOnItemName { get; set; }
        public double AddOnItemPrice { get; set; }
        public int AddOnItemAmount { get; set; }
        public double AddOnItemTotal { get; set; }
        [JsonIgnore]
        public OrderItem OrderItem { get; set; }
        [JsonIgnore]
        public AddOnItem AddOnItem { get; set; }

        public OrderAddOn() : base()
        {
            AddOnItemName = AddOnItem.Title;
            AddOnItemPrice = AddOnItem.Price;
            AddOnItemAmount = 1;
        }

        public void CalculateAddOnItemTotal()
        {
            AddOnItemTotal = AddOnItemPrice * AddOnItemAmount;
        }
    }
}
