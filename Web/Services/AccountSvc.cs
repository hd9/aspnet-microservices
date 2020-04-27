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
using System.Text;

namespace Web.Services
{
    public class AccountSvc : IAccountSvc
    {
        private readonly ILogger<NewsletterSvc> _logger;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _cfg;

        public AccountSvc(HttpClient httpClient, IConfiguration cfg,  ILogger<NewsletterSvc> logger)
        {
            _logger = logger;
            _httpClient = httpClient;
            _cfg = cfg;
        }

        public async Task<Account> GetAccountById(string id)
        {
            var url = $"{_cfg["Services:Account"]}/account/{id}";
            _logger.LogInformation($"Querying account from: '{url}'");

            var resp = await _httpClient.GetAsync(url);

            return JsonConvert.DeserializeObject<Account>(
                await resp.Content.ReadAsStringAsync());
        }

        public async Task<Account> GetAccountByEmail(string email)
        {
            var url = $"{_cfg["Services:Account"]}/account/search?email={email}";
            _logger.LogInformation($"Querying account by email from: '{url}'");

            var resp = await _httpClient.GetAsync(url);

            return JsonConvert.DeserializeObject<Account>(
                await resp.Content.ReadAsStringAsync());
        }

        public async Task CreateAccount(Account acct)
        {
            var url = $"{_cfg["Services:Account"]}/account/";
            _logger.LogInformation($"Creating account from: '{url}'");

            var resp = await _httpClient.PostAsync(
                url, new StringContent(
                    JsonConvert.SerializeObject(acct),
                    Encoding.UTF8,
                    "application/json"));

            // todo :: handle cases when resp.StatusCode != 200
        }

        public async Task UpdateAccount(Account acct)
        {
            var url = $"{_cfg["Services:Account"]}/account/";
            _logger.LogInformation($"Creating account from: '{url}'");

            var resp = await _httpClient.PutAsync(
                url, new StringContent(
                    JsonConvert.SerializeObject(acct),
                    Encoding.UTF8,
                    "application/json"));

            // todo :: handle cases when resp.StatusCode != 200
        }

        public async Task<Account> TrySignIn(SignIn request)
        {
            var acct = await GetAccountByEmail(request.Email);

            // todo :: salt + hash pwd
            return acct != null && acct.Password == request.Password ? acct : null;
        }
    }
}
