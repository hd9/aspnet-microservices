using HildenCo.Core.Contracts.Payment;
using PaymentSvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentSvc.Services
{
    public interface IPaymentGateway
    {
        Task<PaymentGatewayResponse> Process(PaymentGatewayRequest pgr);
    }
}
