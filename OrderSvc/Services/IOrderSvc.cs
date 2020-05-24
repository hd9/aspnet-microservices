using System.Collections.Generic;
using System.Threading.Tasks;
using HildenCo.Core.Contracts.Payment;
using OrderSvc.Models;

namespace OrderSvc.Services
{
    public interface IOrderSvc
    {
        Task<int> SubmitOrder(Order order); 
        Task<IEnumerable<Order>> GetOrdersByAccountId(int accountId);
        Task OnPaymentProcessed(PaymentResponse message);
    }
}