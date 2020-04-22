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
    public class AccountSvc : IAccountSvc
    {
        private readonly ILogger<NewsletterSvc> logger;
        private readonly HttpClient httpClient;
        private readonly IConfiguration cfg;

        public AccountSvc(HttpClient httpClient, IConfiguration cfg,  ILogger<NewsletterSvc> logger)
        {
            this.logger = logger;
            this.httpClient = httpClient;
            this.cfg = cfg;
        }

        public async Task<Account> GetAccount(string id)
        {
            var url = $"{cfg["Services:Account"]}/account/details/{id}";
            logger.LogInformation($"[CatalogSvc] Querying account data from: ${url}");

            var resp = await httpClient.GetAsync(url);

            return JsonConvert.DeserializeObject<Account>(
                await resp.Content.ReadAsStringAsync());
        }

        public async Task CreateAcccount(Account request)
        {
            var url = $"{cfg["Services:Account"]}/account/create/";
            logger.LogInformation($"[CatalogSvc] Creating account from: ${url}");

            var resp = await httpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(request)));

        }

        public async Task SignIn(SignIn request)
        {
            var url = $"{cfg["Services:Account"]}/account/signin/";
            logger.LogInformation($"[CatalogSvc] Signing in account at: ${url}");

            var resp = await httpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(request)));
        }
    }
}
