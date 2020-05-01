using OrderSvc.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderSvc.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrdersByAccountId(int accountId);
        Task<int> Insert(Order order);
    }
}