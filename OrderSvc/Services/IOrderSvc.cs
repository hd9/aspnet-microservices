using System.Collections.Generic;
using System.Threading.Tasks;
using HildenCo.Core.Contracts.Payment;
using HildenCo.Core.Contracts.Shipping;
using OrderSvc.Models;

namespace OrderSvc.Services
{
    public interface IOrderSvc
    {
        Task SubmitOrder(Order order); 
        Task<Order> GetOrderById(int id);
        Task<IEnumerable<Order>> GetOrdersByAccountId(int accountId);
        Task OnPaymentProcessed(PaymentResponse message);
        Task OnShippingProcessed(ShippingResponse message);
    }
}