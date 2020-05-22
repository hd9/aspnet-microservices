﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Web.Models.Order;

namespace Web.Services
{
    public class OrderProxy : IOrderProxy
    {
        private readonly ILogger<OrderProxy> _logger;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _cfg;

        public OrderProxy(HttpClient httpClient, IConfiguration cfg, ILogger<OrderProxy> logger)
        {
            _logger = logger;
            _httpClient = httpClient;
            _cfg = cfg;
        }

        public async Task Submit(Order order)
        {
            var url = $"{_cfg["Services:Order"]}/orders/submit";
            _logger.LogInformation($"Submitting order at '{url}':");

            var resp = await _httpClient.PostAsync(
                url, new StringContent(
                    JsonConvert.SerializeObject(order),
                    Encoding.UTF8,
                    "application/json"));

            // get order number (todo: refactor to OrderNumber)
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
