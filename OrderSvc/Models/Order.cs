using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Infrastructure.Extentions;

namespace OrderSvc.Models
{
    public class Order
    {
        public string Id { get; set; }
        public string AccountId { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public string Currency { get; set; }
        public float Price => (LineItems.HasAny() ? LineItems.Sum(li => li.Qty * li.Price) : 0);
        public float Tax => 0.05f;
        public float Discount { get; set; }
        public float TotalPrice => Price * (1 + Tax);
        public List<LineItem> LineItems { get; set; }
    }
}
