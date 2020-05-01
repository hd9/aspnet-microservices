using System.Collections.Generic;
using System.Threading.Tasks;
using PaymentSvc.Models;

namespace PaymentSvc.Services
{
    public interface IPaymentSvc
    {
        Task<int> SubmitPayment(PaymentInfo account);
        Task<PaymentInfo> GetById(string id);
        Task<PaymentInfo> GetByAccountId(string accountId);
    }
}