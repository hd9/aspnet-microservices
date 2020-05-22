using Web.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Web.Models.Recommendation;

namespace Web.Services
{
    public class RecommendationProxy : IRecommendationProxy
    {
        private readonly ILogger<RecommendationProxy> logger;
        private readonly HttpClient httpClient;
        private readonly IConfiguration cfg;

        public RecommendationProxy(HttpClient httpClient, IConfiguration cfg,  ILogger<RecommendationProxy> logger)
        {
            this.logger = logger;
            this.httpClient = httpClient;
            this.cfg = cfg;
        }

        public async Task<List<Recommendation>> GetByProductSlug(string slug)
        {
            var url = $"{cfg["Services:Recommendation"]}/recommendations/{slug}";
            logger.LogInformation($"[CatalogSvc] Querying products for product '{slug}' from: '{url}'");

            var resp = await httpClient.GetAsync(url);
            var data = await resp.Content.ReadAsStringAsync();

            return JsonConvert.
                DeserializeObject<List<Recommendation>>(data);
        }

        public async Task<List<Recommendation>> GetByAccountId(string accountId)
        {
            var url = $"{cfg["Services:Recommendation"]}/recommendations/account/{accountId}";
            logger.LogInformation($"[CatalogSvc] Querying recommendations for AccountId '{accountId}' from: '{url}'");

            var resp = await httpClient.GetAsync(url);
            var data = await resp.Content.ReadAsStringAsync();

            return JsonConvert.
                DeserializeObject<List<Recommendation>>(data);
        }
    }
}
