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
    public class RecommendationProxy : IRecommendationProxy
    {
        private readonly ILogger<TaxProxy> logger;
        private readonly HttpClient httpClient;
        private readonly IConfiguration cfg;

        public RecommendationProxy(HttpClient httpClient, IConfiguration cfg,  ILogger<TaxProxy> logger)
        {
            this.logger = logger;
            this.httpClient = httpClient;
            this.cfg = cfg;
        }

        public void GetRecommendedProducts(string productId)
        {
            // todo
            
        }
    }
}
