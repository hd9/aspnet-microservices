using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Services
{
    public class AccountProxy : IAccountProxy
    {
        private readonly ILogger<NewsletterProxy> _logger;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _cfg;

        public AccountProxy(HttpClient httpClient, IConfiguration cfg,  ILogger<NewsletterProxy> logger)
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

        public async Task<HttpStatusCode> CreateAccount(Account acct)
        {
            var url = $"{_cfg["Services:Account"]}/account/";
            _logger.LogInformation($"Creating account from: '{url}'");

            var resp = await _httpClient.PostAsync(
                url, new StringContent(
                    JsonConvert.SerializeObject(acct),
                    Encoding.UTF8,
                    "application/json"));

            return resp.StatusCode;
        }

        public async Task<HttpStatusCode> UpdateAccount(AccountDetails acct)
        {
            var url = $"{_cfg["Services:Account"]}/account/";
            _logger.LogInformation($"Creating account from: '{url}'");

            var resp = await _httpClient.PutAsync(
                url, new StringContent(
                    JsonConvert.SerializeObject(acct),
                    Encoding.UTF8,
                    "application/json"));

            return resp.StatusCode;
        }

        public async Task<Account> TrySignIn(SignIn request)
        {
            var acct = await GetAccountByEmail(request.Email);

            // todo :: salt + hash pwd
            return acct != null && acct.Password == request.Password ? acct : null;
        }

        public async Task<HttpStatusCode> UpdatePassword(UpdatePassword changePassword)
        {
            var url = $"{_cfg["Services:Account"]}/account/update-password";
            _logger.LogInformation($"Updating account password at: '{url}'");

            var resp = await _httpClient.PostAsync(
                url, new StringContent(
                    JsonConvert.SerializeObject(changePassword),
                    Encoding.UTF8,
                    "application/json"));

            return resp.StatusCode;
        }
    }
}
