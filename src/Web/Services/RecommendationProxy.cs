using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Web.Infrastructure.Base;
using Web.Models.Recommendation;

namespace Web.Services
{
    public class RecommendationProxy :
        ProxyBase<RecommendationProxy>,
        IRecommendationProxy
    {

        public RecommendationProxy(
            HttpClient httpClient,
            IConfiguration cfg,
            ILogger<RecommendationProxy> logger,
            IDistributedCache cache) :
            base(httpClient, cfg, logger, cache)
        {

        }

        public async Task<List<Recommendation>> GetByProductSlug(string slug)
        {
            var url = $"/recommendations/{slug}";

            return await GetAsync<List<Recommendation>>(
                "product", slug, url);
        }

        public async Task<List<Recommendation>> GetByAccountId(string accountId)
        {
            var url = $"/recommendations/account/{accountId}";

            return await GetAsync<List<Recommendation>>(
                "account", accountId, url);
        }

    }
}
