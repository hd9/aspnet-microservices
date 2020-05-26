using HildenCo.Core.Contracts.Shipping;
using MassTransit;
using ShippingSvc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingSvc.Consumers
{
    /// <summary>
    /// Requests a new Shipping from the Shipping service
    /// </summary>
    public class ShippingRequestConsumer
        : IConsumer<ShippingRequest>
    {

        readonly IShippingSvc _svc;

        public ShippingRequestConsumer(IShippingSvc svc)
        {
            _svc = svc;
        }

        public async Task Consume(ConsumeContext<ShippingRequest> context)
        {
            await _svc.RequestShipping(context.Message);
        }
    }
}
