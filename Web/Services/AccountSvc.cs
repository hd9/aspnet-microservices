using Web.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using Core.Infrastructure.Extentions;

namespace Web.Services
{
    public class AccountSvc : IAccountSvc
    {
        private readonly ILogger<NewsletterSvc> _logger;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _cfg;
        private readonly List<Account> _accounts;
        private readonly List<Order> _orders;

        public AccountSvc(HttpClient httpClient, IConfiguration cfg,  ILogger<NewsletterSvc> logger)
        {
            _logger = logger;
            _httpClient = httpClient;
            _cfg = cfg;

            _accounts = new List<Account>
            {
                new Account { Name = "User 1", Email = "usr1@mail.com", Password = "pwd01", Id = "usr1xx1", SubscribedToNewsletter = true },
                new Account { Name = "User 2", Email = "usr2@mail.com", Password = "pwd02", Id = "usr2xx2" },
                new Account { Name = "User 3", Email = "usr3@mail.com", Password = "pwd03", Id = "usr3xx3" }
            };

            _orders = new List<Order>
            {
                new Order {
                    Id = "ox1abc",
                    AccountId = "usr1xx1",
                    CreatedOn = DateTime.UtcNow.AddDays(-13),
                    Currency = "CAD",
                    Tax = 0.05f,
                    LineItems = new List<LineItem> {
                        new LineItem { Id = "iphone-X11", Name = "iPhone X", Price = 999, Qty = 1 },
                        new LineItem { Id = "sb-789", Name = "Soccer Ball", Price = 15, Qty = 1 },
                        new LineItem { Id = "iphone-X11-case1", Name = "iPhone X Case", Price = 9.99f, Qty = 3 },
                    }
                }
            };
        }

        public async Task<Account> GetAccountByEmail(string email)
        {
            return _accounts.FirstOrDefault(a => a.Email == email);
        }

        public async Task<Account> GetAccount(string id)
        {
            return _accounts.FirstOrDefault(a => a.Id == id);


            // todo :: implement microservice
            var url = $"{_cfg["Services:Account"]}/account/details/{id}";
            _logger.LogInformation($"[CatalogSvc] Querying account data from: ${url}");

            var resp = await _httpClient.GetAsync(url);

            return JsonConvert.DeserializeObject<Account>(
                await resp.Content.ReadAsStringAsync());
        }

        public async Task UpdateAccount(Account acct)
        {
            if (acct == null)
                return;

            // todo :: implement http call
        }

        public async Task CreateAccount(Account acct)
        {
            var url = $"{_cfg["Services:Account"]}/account/create/";
            _logger.LogInformation($"[CatalogSvc] Creating account from: ${url}");

            var resp = await _httpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(acct)));

        }

        public async Task<Account> TrySignIn(SignIn request)
        {
            if (request == null || !request.IsValid())
                return null;

            var acct = await GetAccountByEmail(request.Email);

            return acct != null && acct.Password == request.Password ? acct : null;
        }

        public async Task<IList<Order>> GetOrders(string acctId)
        {
            return _orders.Where(o => o.AccountId == acctId).ToList();

            // todo :: implement rest call
        }
    }
}
