using Web.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Web.Services
{
    public class OrderSvc : IOrderSvc
    {
        private readonly ILogger<OrderSvc> logger;
        private readonly HttpClient httpClient;
        private readonly IConfiguration cfg;
        private static readonly List<Order> _orders = new List<Order>();

        public OrderSvc(HttpClient httpClient, IConfiguration cfg,  ILogger<OrderSvc> logger)
        {
            this.logger = logger;
            this.httpClient = httpClient;
            this.cfg = cfg;
        }

        public async Task Submit(Order o)
        {
            // todo :: add rest call

            o.Id = $"O-{DateTime.UtcNow.Year}-{Guid.NewGuid().ToString().Replace("-", "").Substring(0,10).ToUpper()}";
            o.Currency = "CAD";

            _orders.Add(o);
        }

        public async Task<List<Order>> GetOrders(string accountId)
        {
            // todo :: add rest call
            return _orders.Where(o => o.AccountId == accountId).ToList();
        }

        public Order GetOrder(string id)
        {
            // todo :: add rest call
            return _orders.FirstOrDefault(o => o.Id == id);
        }
    }
}
