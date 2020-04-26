﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Services
{
    public class CatalogSvc : ICatalogSvc
    {
        private readonly ILogger<CatalogSvc> logger;
        private readonly HttpClient httpClient;
        private readonly IConfiguration cfg;

        public CatalogSvc(HttpClient httpClient, IConfiguration cfg, ILogger<CatalogSvc> logger)
        {
            this.logger = logger;
            this.httpClient = httpClient;
            this.cfg = cfg;
        }

        public async Task<List<Product>> GetProductsByCategory(string slug)
        {
            var url = $"{cfg["Services:Catalog"]}/products/{slug}";
            logger.LogInformation($"[CatalogSvc] Querying products by category: '{slug}' from: '{url}'");

            var resp = await httpClient.GetAsync(url);
            var data = await resp.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Product>>(data);
        }

        public async Task<List<Category>> GetCategories()
        {
            var url = $"{cfg["Services:Catalog"]}/categories";
            logger.LogInformation($"[CatalogSvc] Querying categories from '{url}'");

            var resp = await httpClient.GetAsync(url);
            var data = await resp.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Category>>(data);
        }

        public async Task<Product> GetProductBySlug(string slug)
        {
            var url = $"{cfg["Services:Catalog"]}/product/{slug}";
            logger.LogInformation($"[CatalogSvc] Querying product '{slug}' from: '{url}'");

            var resp = await httpClient.GetAsync(url);
            var data = await resp.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Product>(data);
        }

        public async Task<Category> GetCategory(string slug)
        {
            var url = $"{cfg["Services:Catalog"]}/categories/{slug}";
            logger.LogInformation($"[CatalogSvc] Querying category '{slug}' from: '{url}'");

            var resp = await httpClient.GetAsync(url);
            var data = await resp.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Category>(data);
        }
    }
}
