using OrderSvc.Models;
using OrderSvc.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderSvc.Services
{
    public class OrderSvc : IOrderSvc
    {
        private readonly IOrderRepository _repo;

        public OrderSvc(IOrderRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Order>> GetOrdersByAccountId(int accountId)
        {
            return await _repo.GetOrdersByAccountId(accountId);
        }

        public async Task<int> SubmitOrder(Order order)
        {
            return await _repo.Insert(order);
        }

    }
}
