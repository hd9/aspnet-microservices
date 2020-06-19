using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountSvc.Models;
using AccountSvc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Web.Models;

namespace AccountSvc.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        readonly IAccountSvc _svc;
        readonly IConfiguration cfg;
        const string help = @"The Account service is alive! Try GET /api/v1/account/{account-id}";

        public AccountController(IAccountSvc svc, IConfiguration cfg)
        {
            this._svc = svc;
            this.cfg = cfg;
        }

        [Route("/ping")]
        public IActionResult Ping()
        {
            return Ok();
        }

        [Route("/help")]
        public IActionResult Help()
        {
            return Ok(help);
        }

        [HttpPost]
        [Route("/api/v1/account/")]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccount account)
        {
            await _svc.CreateAccount(account);
            return Ok();
        }

        [HttpPut]
        [Route("/api/v1/account/")]
        public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccount account)
        {
            await _svc.UpdateAccount(account);
            return Ok();
        }

        [HttpPost]
        [Route("/api/v1/account/update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePassword updPassword)
        {
            await _svc.UpdatePassword(updPassword);
            return Ok();
        }

        [HttpPost]
        [Route("/api/v1/account/signin")]
        public async Task<IActionResult> SignIn([FromBody] SignIn signin)
        {
            var acct = await _svc.TrySignIn(signin);
            return Ok(acct);
        }

        [Route("/api/v1/account/{id}")]
        public async Task<Account> GetAccount(string id)
        {
            return await _svc.GetAccountById(id);
        }

        [Route("/api/v1/account/search")]
        public async Task<Account> GetAccountByEmail(string email)
        {
            return await _svc.GetAccountByEmail(email);
        }

        [Route("/api/v1/account/address/{addrId}")]
        public async Task<Address> GetAddressById(string addrId)
        {
            return await _svc.GetAddressById(addrId);
        }

        [Route("/api/v1/account/address/")]
        [HttpPost]
        public async Task AddAddress([FromBody]Address addr)
        {
            await _svc.AddAddress(addr);
        }

        [Route("/api/v1/account/address/")]
        [HttpPut]
        public async Task UpdateAddress([FromBody]Address addr)
        {
            await _svc.UpdateAddress(addr);
        }

        [Route("/api/v1/account/address/{addressId}")]
        [HttpDelete]
        public async Task RemoveAddress(string addressId)
        {
            await _svc.RemoveAddress(addressId);
        }

        [Route("/api/v1/account/address/search")]
        public async Task<IList<Address>> GetAddressesByAccountId(string accountId)
        {
            return await _svc.GetAddressesByAccountId(accountId);
        }

        [Route("/api/v1/account/address/default")]
        public async Task SetDefaultAddress(string accountId, int addressId)
        {
            await _svc.SetDefultAddress(accountId, addressId);
        }

        [Route("/api/v1/account/payment/{pmtId}")]
        public async Task<PaymentInfo> GetPaymentInfoById(string pmtId)
        {
            return await _svc.GetPaymentInfoById(pmtId);
        }

        [HttpPost]
        [Route("/api/v1/account/payment/")]
        public async Task AddPaymentInfo([FromBody]PaymentInfo pmtInfo)
        {
            await _svc.AddPaymentInfo(pmtInfo);
        }

        [HttpPut]
        [Route("/api/v1/account/payment/")]
        public async Task UpdatePaymentInfo([FromBody]PaymentInfo pmtInfo)
        {
            await _svc.UpdatePaymentInfo(pmtInfo);
        }

        [Route("/api/v1/account/payment/{pmtId}")]
        [HttpDelete]
        public async Task RemovePaymentInfo(string pmtId)
        {
            await _svc.RemovePaymentInfo(pmtId);
        }

        [Route("/api/v1/account/payment/search")]
        public async Task<IList<PaymentInfo>> GetPaymentInfosByAccountId(string accountId)
        {
            return await _svc.GetPaymentInfosByAccountId(accountId);
        }

        [Route("/api/v1/account/payment/default")]
        public async Task SetDefaultPaymentInfo(string accountId, int pmtId)
        {
            await _svc.SetDefaultPaymentInfo(accountId, pmtId);
        }

        [Route("/api/v1/account/history/{acctId}")]
        public async Task<IList<AccountHistory>> GetAccountHistory(string acctId)
        {
            return await _svc.GetAccountHistory(acctId);
        }
    }
}
