using System.Collections.Generic;
using System.Threading.Tasks;
using HildenCo.Core.Contracts.Payment;
using PaymentSvc.Models;

namespace PaymentSvc.Services
{
    public interface IPaymentSvc
    {
        Task RequestPayment(PaymentRequest paymentRequestccount);
        Task<Payment> GetById(string id);
        Task<Payment> GetByAccountId(string accountId);
    }
}