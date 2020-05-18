using Core.Events.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Events.Orders
{
    public class OrderSubmitted : EventBase
    {
        public List<string> OrderIds { get; set; }
    }
}
