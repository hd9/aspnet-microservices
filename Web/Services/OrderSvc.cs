using Web.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Web.Infrastructure.Settings;
using Web.Infrastructure.Global;

namespace Web.Services
{
    public class OrderSvc : IOrderSvc
    {
        private readonly ILogger<OrderSvc> _logger;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _cfg;
        private readonly StoreSettings _storeSettings;

        public OrderSvc(HttpClient httpClient, IConfiguration cfg, ILogger<OrderSvc> logger)
        {
            _logger = logger;
            _httpClient = httpClient;
            _cfg = cfg;
            _storeSettings = Site.StoreSettings;
        }

        public async Task Submit(Order order)
        {
            order.Currency = _storeSettings.Currency;
            order.Tax = _storeSettings.Tax;

            var url = $"{_cfg["Services:Order"]}/orders/submit";
            _logger.LogInformation($"Submitting order at '{url}':");

            var resp = await _httpClient.PostAsync(
                url, new StringContent(
                    JsonConvert.SerializeObject(order),
                    Encoding.UTF8,
                    "application/json"));

            // get order number
            order.Id = JsonConvert.DeserializeObject<int>(
                await resp.Content.ReadAsStringAsync());
        }

        public async Task<List<Order>> GetOrdersByAccountId(string accountId)
        {
            var url = $"{_cfg["Services:Order"]}/orders/{accountId}";
            _logger.LogInformation($"Querying orders by accountId at: '{url}'");

            var resp = await _httpClient.GetAsync(url);

            return JsonConvert.DeserializeObject<List<Order>>(
                await resp.Content.ReadAsStringAsync());
        }
    }
}
