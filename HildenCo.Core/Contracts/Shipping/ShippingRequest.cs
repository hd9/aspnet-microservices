using HildenCo.Core.Contracts.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace HildenCo.Core.Contracts.Shipping
{
    public class ShippingRequest : CommandBase
    {
        public int AccountId { get; set; }

        public int OrderId { get; set; }

        // todo :: add address fields
    }
}
