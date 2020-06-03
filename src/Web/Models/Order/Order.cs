using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microservices.Core.Infrastructure.Extensions;
using Web.Models.Account;

namespace Web.Models.Order
{
    public class Order
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public OrderStatus Status { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public ShippingStatus ShippingStatus { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public string Currency { get; set; }
        public decimal Price => (LineItems.HasAny() ? LineItems.Sum(li => li.Qty * li.Price) : 0);
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public decimal Shipping { get; set; }
        public decimal TotalPrice => Math.Round(Price * (1 + Tax) + Shipping - Discount, 2);
        public List<LineItem> LineItems { get; set; }
        public PaymentInfo PaymentInfo { get; set; }
        public Address ShippingInfo { get; set; }

        public bool IsValidForSubmit() =>
            AccountId > 0 &&
            Currency.HasValue(3) &&
            Tax > 0 && Tax < 1 &&
            LineItems.HasAny() &&
            Price > 0 &&
            ShippingInfo.IsValid() &&
            PaymentInfo.IsValid() &&
            LineItems.Count > 0;
    }
}
