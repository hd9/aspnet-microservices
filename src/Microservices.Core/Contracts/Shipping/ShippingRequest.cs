using Microservices.Core.Contracts.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Core.Contracts.Shipping
{
    public class ShippingRequest : CommandBase
    {
        public int AccountId { get; set; }

        public int OrderId { get; set; }
        
        public string Name { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string Street { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public string Country { get; set; }

        public ShippingStatus Status { get; set; }

        public ShippingProvider Provider { get; set; }
    }
}
