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
using Web.Infrastructure.Base;
using Microsoft.Extensions.Caching.Distributed;

namespace Web.Services
{
    public class NewsletterProxy : 
        ProxyBase<NewsletterProxy>,
        INewsletterProxy
    {
        public NewsletterProxy(
            HttpClient httpClient,
            IConfiguration cfg,
            ILogger<NewsletterProxy> logger,
            IDistributedCache cache) :
            base(httpClient, cfg, logger, cache)
        {

        }

        public async Task Signup(NewsletterSignUp signup)
        {
            await PostAsync("signup", signup, "/api/v1/signup");
        }
    }
}
