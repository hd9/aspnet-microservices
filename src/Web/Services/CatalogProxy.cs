using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Web.Infrastructure.Base;
using Web.Models;
using Web.Models.Catalog;

namespace Web.Services
{
    public class CatalogProxy :
        ProxyBase<CatalogProxy>,
        ICatalogProxy
    {
        public CatalogProxy(
            HttpClient httpClient,
            IConfiguration cfg,
            ILogger<CatalogProxy> logger,
            IDistributedCache cache) :
            base(httpClient, cfg, logger, cache)
        {

        }

        public async Task<List<Product>> GetProductsByCategory(string slug)
        {
            var endpoint = $"/products/{slug}";
            return await GetAsync<List<Product>>(
                "product", slug, endpoint);
        }

        public async Task<List<Category>> GetCategories()
        {
            return await GetAsync<List<Category>>(
                "categories", null, "/categories");
        }

        public async Task<Product> GetProductBySlug(string slug)
        {
            var endpoint = $"/product/{slug}";
            return await GetAsync<Product>(
                "product", slug, endpoint);
        }

        public async Task<Category> GetCategory(string slug)
        {
            var endpoint = $"/categories/{slug}";
            return await GetAsync<Category>(
                "categories", slug, endpoint);
        }
    }
}
