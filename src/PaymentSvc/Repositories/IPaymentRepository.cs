using PaymentSvc.Models;
using PaymentSvc.Services;
using System.Threading.Tasks;

namespace PaymentSvc.Repositories
{
    public interface IPaymentRepository
    {
        Task InsertPayment(Payment pmt);
        Task UpdatePayment(Payment pmt);
        Task InsertPaymentRequest(PaymentGatewayRequest pgr);
        Task<Payment> GetById(string id);
        Task<Payment> GetByAccountId(string email);
    }
}