using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microservices.Core.Infrastructure.Extensions;

namespace OrderSvc.Models
{
    public class Order
    {
        // For simplicity, we're making Id public 
        // but it shouldn't be exposed to any external service
        // as it's part of the internal information for OrderSvc.
        // All external references should use the field `Number`
        public int Id { get; set; }
        public string Number { get; set; }
        public int AccountId { get; set; }
        public OrderStatus Status { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public ShippingStatus ShippingStatus { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public string Currency { get; set; }
        public decimal Price => (LineItems.HasAny() ? LineItems.Sum(li => li.Qty * li.Price) : 0);
        public decimal Tax => 0.05M;
        public decimal Discount { get; set; }
        public decimal Shipping { get; set; }
        public decimal TotalPrice => Math.Round(Price * (1 + Tax) + Shipping - Discount, 2);
        public List<LineItem> LineItems { get; set; }
        public PaymentInfo PaymentInfo { get; set; }
        public ShippingInfo ShippingInfo { get; set; }

        /// <summary>
        /// Basic override for logging purposes
        /// </summary>
        /// <returns></returns>
        public override string ToString() =>
            $"Id: '{Id}', Number: '{Number}', AccountId: '{AccountId}', Price: {Currency} {TotalPrice}";
    }
}
