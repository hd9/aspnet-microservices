using HildenCo.Core.Contracts.Payment;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace OrderSvc.Consumers
{
    public class PaymentResponseConsumer
        : IConsumer<PaymentResponse>
    {
        public async Task Consume(ConsumeContext<PaymentResponse> context)
        {
            // todo :: update order
        }
    }
}
