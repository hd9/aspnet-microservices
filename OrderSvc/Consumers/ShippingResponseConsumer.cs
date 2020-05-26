using HildenCo.Core.Contracts.Payment;
using HildenCo.Core.Contracts.Shipping;
using MassTransit;
using OrderSvc.Services;
using System;
using System.Threading.Tasks;

namespace OrderSvc.Consumers
{
    /// <summary>
    /// ShippingResponseConsumer consumes a shipping response
    /// and triggers the workflow so offers can be processed.
    /// </summary>
    public class ShippingResponseConsumer
        : IConsumer<ShippingResponse>
    {

        readonly IOrderSvc _svc;

        public ShippingResponseConsumer(IOrderSvc svc)
        {
            _svc = svc;
        }

        public async Task Consume(ConsumeContext<ShippingResponse> context)
        {
            await _svc.OnShippingProcessed(context.Message);
        }
    }
}
