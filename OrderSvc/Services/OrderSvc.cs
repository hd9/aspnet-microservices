using HildenCo.Core.Contracts.Orders;
using HildenCo.Core.Contracts.Payment;
using MassTransit;
using OrderSvc.Models;
using OrderSvc.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderSvc.Services
{
    public class OrderSvc : IOrderSvc
    {

        readonly IOrderRepository _repo;
        readonly IBusControl _bus;

        public OrderSvc(IOrderRepository repo, IBusControl bus)
        {
            _repo = repo;
            _bus = bus;
        }

        public async Task<IEnumerable<Order>> GetOrdersByAccountId(int accountId)
        {
            return await _repo.GetOrdersByAccountId(accountId);
        }

        public async Task<int> SubmitOrder(Order order)
        {
            var orderId = await _repo.Insert(order);

            await _bus.Publish(
                new OrderSubmitted
                {
                    Slugs = order.LineItems.Select(li => li.Slug).ToList()
                });

            // todo :: automapper
            await _bus.Publish(
                new PaymentRequest
                {
                    AccountId = order.AccountId,
                    OrderId = orderId,
                    Currency = order.Currency,
                    Amount = order.TotalPrice,
                    Method = order.PaymentInfo.Method.ToString(),
                    Name = order.PaymentInfo.Name,
                    Number = order.PaymentInfo.Number,
                    ExpDate = order.PaymentInfo.ExpDate,
                    CVV = order.PaymentInfo.CVV,
                    FakeDelay = 100,
                    FakeResult = true
                }
            );

            return orderId;
        }
    }
}
