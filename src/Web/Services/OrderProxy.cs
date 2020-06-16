using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Web.Infrastructure.Base;
using Web.Models.Account;
using Web.Models.Order;

namespace Web.Services
{

    public class OrderProxy :
        ProxyBase<OrderProxy>,
        IOrderProxy
    {
        public OrderProxy(
            HttpClient httpClient,
            IConfiguration cfg,
            ILogger<OrderProxy> logger,
            IDistributedCache cache) :
            base(httpClient, cfg, logger, cache)
        {

        }

        public async Task Submit(Order order)
        {
            var on = await PostAsync<OrderNumberResponse>(
                "order", order, "/api/v1/orders/submit");

            order.Number = on.Number;
        }

        public async Task<List<Order>> GetOrdersByAccountId(string accountId)
        {
            var endpoint = $"/api/v1/orders/{accountId}";
            return await GetAsync<List<Order>>(
                "accountid", accountId, endpoint);
        }
    }
}
