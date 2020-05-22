using HildenCo.Core.Contracts.Base;
using System.Collections.Generic;

namespace HildenCo.Core.Contracts.Orders
{
    public class OrderSubmitted : EventBase
    {
        public List<string> Slugs { get; set; }
    }
}
