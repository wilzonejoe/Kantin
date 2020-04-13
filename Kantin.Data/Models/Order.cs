using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Kantin.Data.Enumerable;
using System.ComponentModel.DataAnnotations;
using Core.Models.Abstracts;
using System.Linq;

namespace Kantin.Data.Models
{
    public class Order : ValidationEntity
    {
        [Required]
        public Guid AccountId { get; set; }
        public int OrderNumber { get; set; }
        public double OrderTotal { get; set; }
        public DateTime OrderDateTime { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        [JsonIgnore]
        public ICollection<OrderItem> OrderItems { get; set; }
        [JsonIgnore]
        public Account Account { get; set; }

        public Order()
        {
            OrderItems = new List<OrderItem>();
        }

        public void CalculateTotal()
        {
            OrderTotal = OrderItems.Sum(orderItem => orderItem.MenuItemTotal);
        }
    }
}
