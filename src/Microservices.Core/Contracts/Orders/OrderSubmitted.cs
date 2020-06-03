using Microservices.Core.Contracts.Base;
using System.Collections.Generic;

namespace Microservices.Core.Contracts.Orders
{
    public class OrderSubmitted : EventBase
    {
        public List<string> Slugs { get; set; }
    }
}
