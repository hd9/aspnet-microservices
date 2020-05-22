using HildenCo.Core.Contracts.Orders;
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

            return orderId;
        }

    }
}
