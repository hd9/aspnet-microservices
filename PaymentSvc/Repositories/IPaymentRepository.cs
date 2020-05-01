using PaymentSvc.Models;
using System.Threading.Tasks;

namespace PaymentSvc.Repositories
{
    public interface IPaymentRepository
    {
        Task<int> Insert(PaymentInfo order);
        Task<PaymentInfo> GetById(string id);
        Task<PaymentInfo> GetByAccountId(string email);
    }
}