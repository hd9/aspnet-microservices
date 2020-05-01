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
    public class NewsletterProxy : INewsletterProxy
    {
        private readonly ILogger<NewsletterProxy> logger;
        private readonly HttpClient httpClient;
        private readonly IConfiguration cfg;

        public NewsletterProxy(HttpClient httpClient, IConfiguration cfg,  ILogger<NewsletterProxy> logger)
        {
            this.logger = logger;
            this.httpClient = httpClient;
            this.cfg = cfg;
        }

        public void Signup(NewsletterSignUp signup)
        {
            var url = $"{cfg["Services:Newsletter"]}/signup";
            logger.LogInformation($"[NewsletterSvc] Posting new signup form to: ${url}");

            var data = new StringContent(JsonConvert.SerializeObject(signup), System.Text.Encoding.UTF8, "application/json");
            var resp = httpClient.PostAsync(url, data).GetAwaiter().GetResult();

            logger.LogInformation($"[NewsletterSvc] Signup response | Status Code: ${resp.StatusCode}, Headers: {resp.Headers}");
        }
    }
}
