using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Web.Infrastructure.Base;
using Web.Models.Account;
using Web.Models.Order;

namespace Web.Services
{
    public class AccountProxy :
        ProxyBase<AccountProxy>,
        IAccountProxy
    {
        public AccountProxy(
            HttpClient httpClient,
            IConfiguration cfg,
            ILogger<AccountProxy> logger,
            IDistributedCache cache) :
            base(httpClient, cfg, logger, cache)
        {
            // because account info is more volatile than catalog, etc
            // we want its cache to be refreshed more frequently
            cacheOptions = new DistributedCacheEntryOptions {
                SlidingExpiration = TimeSpan.FromSeconds(10)
            };
        }

        public async Task<Account> GetAccountById(string id)
        {
            var endpoint = $"/api/v1/account/{id}";
            return await GetAsync<Account>(
                "account", id, endpoint);
        }

        public async Task<Account> GetAccountByEmail(string email)
        {
            var endpoint = $"/api/v1/account/search?email={email}";
            return await GetAsync<Account>(
                "email", email, endpoint);
        }

        public async Task<HttpStatusCode> CreateAccount(Account acct)
        {
            return await PostAsync(
                "account", acct, "/api/v1/account/");
        }

        public async Task<HttpStatusCode> UpdateAccount(AccountDetails acct)
        {
            return await PutAsync(
                "account-upd", acct, "/api/v1/account/");
        }

        public async Task<Account> TrySignIn(SignIn signIn)
        {
            return await PostAsync<Account>(
                "signin", signIn, "/api/v1/account/signin");
        }

        public async Task<HttpStatusCode> UpdatePassword(UpdatePassword updPassword)
        {
            return await PostAsync(
                "update-password",
                updPassword,
                "/api/v1/account/update-password");
        }

        public async Task<Address> GetAddressById(string addrId)
        {
            var endpoint = $"/api/v1/account/address/{addrId}";
            return await GetAsync<Address>(
                "address", addrId, endpoint);
        }

        public async Task<HttpStatusCode> AddAddress(Address addr)
        {
            return await PostAsync("address", addr, "/api/v1/account/address");
        }

        public async Task<HttpStatusCode> UpdateAddress(Address addr)
        {
            return await PutAsync("address-upd", addr, "/api/v1/account/address");
        }

        public async Task<HttpStatusCode> RemoveAddress(string addressId)
        {
            return await DeleteAsync(
                "address-del",
                $"/api/v1/account/address/{addressId}");
        }

        public async Task<IList<Address>> GetAddressesByAccountId(string acctId)
        {
            var endpoint = $"/api/v1/account/address/search?accountId={acctId}";
            return await GetAsync<IList<Address>>(
                "address-by-accountid", acctId, endpoint);
        }

        public async Task<HttpStatusCode> SetDefaultAddress(string acctId, int addressId)
        {
            var endpoint = $"/api/v1/account/address/default?accountId={acctId}&addressId={addressId}";
            return await PostAsync(
                "address-default", 
                new { acctId, addressId }, 
                endpoint);
        }

        public async Task<PaymentInfo> GetPaymentInfoById(string pmtId)
        {
            var endpoint = $"/api/v1/account/payment/{pmtId}";
            return await GetAsync<PaymentInfo>(
                "payment", pmtId, endpoint);
        }

        public async Task<IList<PaymentInfo>> GetPaymentInfos(string accountId)
        {
            var endpoint = $"/api/v1/account/payment/search?accountId={accountId}";
            return await GetAsync<IList<PaymentInfo>>(
                "payment-accountid", accountId, endpoint);
        }

        public async Task<HttpStatusCode> AddPayment(PaymentInfo pmtInfo)
        {
            return await PostAsync(
                "payment-add",
                pmtInfo,
                "/api/v1/account/payment");
        }

        public async Task<HttpStatusCode> RemovePayment(string pmtId)
        {
            return await DeleteAsync(
                "payment-remove",
                $"/api/v1/account/payment/{pmtId}");
        }

        public async Task<HttpStatusCode> SetDefaultPayment(string acctId, int pmtId)
        {
            var endpoint = $"/api/v1/account/payment/default?accountId={acctId}&pmtId={pmtId}";
            return await PostAsync(
                "payment-default",
                new { acctId, pmtId },
                endpoint);
        }

        public async Task<HttpStatusCode> UpdatePayment(PaymentInfo pmtInfo)
        {
            return await PutAsync(
                "payment-upd",
                pmtInfo,
                "/api/v1/account/payment");
        }

        public async Task<IList<AccountHistory>> GetAccountHistory(string acctId)
        {
            var endpoint = $"/api/v1/account/history/{acctId}";
            return await GetAsync<IList<AccountHistory>>(
                "account-history", acctId, endpoint);
        }
    }
}
