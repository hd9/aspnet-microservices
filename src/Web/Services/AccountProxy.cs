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

        }

        public async Task<Account> GetAccountById(string id)
        {
            var endpoint = $"/account/{id}";
            return await GetAsync<Account>(
                "account", id, endpoint);
        }

        public async Task<Account> GetAccountByEmail(string email)
        {
            var endpoint = $"/account/search?email={email}";
            return await GetAsync<Account>(
                "email", email, endpoint);
        }

        public async Task<HttpStatusCode> CreateAccount(Account acct)
        {
            return await PostAsync("account", acct, "/account/");
        }

        public async Task<HttpStatusCode> UpdateAccount(AccountDetails acct)
        {
            return await PostAsync(
                "account-upd", acct, "/account/");
        }

        public async Task<Account> TrySignIn(SignIn signIn)
        {
            return await PostAsync<Account>(
                "signin", signIn, "/account/signin");
        }

        public async Task<HttpStatusCode> UpdatePassword(UpdatePassword updPassword)
        {
            return await PostAsync(
                "update-password",
                updPassword,
                "/account/update-password");
        }

        public async Task<Address> GetAddressById(string addrId)
        {
            var endpoint = $"/account/address/{addrId}";
            return await GetAsync<Address>(
                "address", addrId, endpoint);
        }

        public async Task<HttpStatusCode> AddAddress(Address addr)
        {
            return await PostAsync("address", addr, "/account/address");
        }

        public async Task<HttpStatusCode> UpdateAddress(Address addr)
        {
            return await PutAsync("address-upd", addr, "/account/address");
        }

        public async Task<HttpStatusCode> RemoveAddress(string addressId)
        {
            return await DeleteAsync(
                "address-del",
                $"/account/address/{addressId}");
        }

        public async Task<IList<Address>> GetAddressesByAccountId(string acctId)
        {
            var endpoint = $"/account/address/search?accountId={acctId}";
            return await GetAsync<IList<Address>>(
                "address-by-accountid", acctId, endpoint);
        }

        public async Task<HttpStatusCode> SetDefaultAddress(string acctId, int addressId)
        {
            var endpoint = $"/account/address/default?accountId={acctId}&addressId={addressId}";
            return await PostAsync(
                "address-default", 
                new { acctId, addressId }, 
                endpoint);
        }

        public async Task<PaymentInfo> GetPaymentInfoById(string pmtId)
        {
            var endpoint = $"/account/payment/{pmtId}";
            return await GetAsync<PaymentInfo>(
                "payment", pmtId, endpoint);
        }

        public async Task<IList<PaymentInfo>> GetPaymentInfos(string accountId)
        {
            var endpoint = $"/account/payment/search?accountId={accountId}";
            return await GetAsync<IList<PaymentInfo>>(
                "payment-accountid", accountId, endpoint);
        }

        public async Task<HttpStatusCode> AddPayment(PaymentInfo pmtInfo)
        {
            return await PostAsync(
                "payment-add",
                pmtInfo,
                "/account/payment");
        }

        public async Task<HttpStatusCode> RemovePayment(string pmtId)
        {
            return await DeleteAsync(
                "payment-remove",
                $"/account/payment/{pmtId}");
        }

        public async Task<HttpStatusCode> SetDefaultPayment(string acctId, int pmtId)
        {
            var endpoint = $"/account/payment/default?accountId={acctId}&pmtId={pmtId}";
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
                "/account/payment");
        }

        public async Task<IList<AccountHistory>> GetAccountHistory(string acctId)
        {
            var endpoint = $"/account/history/{acctId}";
            return await GetAsync<IList<AccountHistory>>(
                "account-history", acctId, endpoint);
        }
    }
}
