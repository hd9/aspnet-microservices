using HildenCo.Core.Contracts.Payment;
using MassTransit;
using PaymentSvc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentSvc.Consumers
{
    /// <summary>
    /// Requests a new payment from the Payment service
    /// </summary>
    public class PaymentRequestConsumer
        : IConsumer<PaymentRequest>
    {

        readonly IPaymentSvc _svc;

        public PaymentRequestConsumer(IPaymentSvc svc)
        {
            _svc = svc;
        }

        public async Task Consume(ConsumeContext<PaymentRequest> context)
        {
            await _svc.RequestPayment(context.Message);
        }
    }
}
