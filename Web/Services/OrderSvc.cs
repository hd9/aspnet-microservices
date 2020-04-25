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

        public OrderSvc(HttpClient httpClient, IConfiguration cfg,  ILogger<OrderSvc> logger)
        {
            this.logger = logger;
            this.httpClient = httpClient;
            this.cfg = cfg;
        }

        public async Task Submit(Order request)
        {
            // todo
        }

        public void GetOrders(string accountId)
        {
            // todo
        }

        public void GetOrder(string id)
        {
            // todo
        }
    }
}
