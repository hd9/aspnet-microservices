using HildenCo.Core.Contracts.Payment;
using MassTransit;
using OrderSvc.Services;
using System;
using System.Threading.Tasks;

namespace OrderSvc.Consumers
{
    /// <summary>
    /// Consumes a payment response, a response from the payment service
    /// regarding a payment requested previously.
    /// </summary>
    public class PaymentResponseConsumer
        : IConsumer<PaymentResponse>
    {

        readonly IOrderSvc _svc;

        public PaymentResponseConsumer(IOrderSvc svc)
        {
            _svc = svc;
        }

        public async Task Consume(ConsumeContext<PaymentResponse> context)
        {
            await _svc.OnPaymentProcessed(context.Message);
        }
    }
}
