using Microservices.Core.Contracts.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Core.Contracts.Shipping
{
    public class ShippingResponse : CommandBase
    {
        public int AccountId { get; set; }

        public int OrderId { get; set; }
        
        public string Number { get; set; }

        public ShippingStatus Status { get; set; }

    }
}
