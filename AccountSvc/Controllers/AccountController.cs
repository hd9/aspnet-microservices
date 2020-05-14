using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountSvc.Models;
using AccountSvc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AccountSvc.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountSvc _svc;
        private readonly IConfiguration cfg;
        const string help = @"The service is alive!";

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
        [Route("/account/")]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccount account)
        {
            await _svc.CreateAccount(account);
            return Ok();
        }

        [HttpPut]
        [Route("/account/")]
        public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccount account)
        {
            await _svc.UpdateAccount(account);
            return Ok();
        }


        [HttpPost]
        [Route("/account/update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePassword updPassword)
        {
            await _svc.UpdatePassword(updPassword);
            return Ok();
        }

        [Route("/account/{id}")]
        public async Task<Account> GetAccount(string id)
        {
            return await _svc.GetAccountById(id);
        }

        [Route("/account/search")]
        public async Task<Account> GetAccountByEmail(string email)
        {
            return await _svc.GetAccountByEmail(email);
        }

        [Route("/account/address/{addrId}")]
        public async Task<Address> GetAddressById(string addrId)
        {
            return await _svc.GetAddressById(addrId);
        }

        [Route("/account/address/")]
        [HttpPost]
        public async Task AddAddress([FromBody]Address addr)
        {
            await _svc.AddAddress(addr);
        }

        [Route("/account/address/")]
        [HttpPut]
        public async Task UpdateAddress([FromBody]Address addr)
        {
            await _svc.UpdateAddress(addr);
        }

        [Route("/account/address/{addressId}")]
        [HttpDelete]
        public async Task RemoveAddress(string addressId)
        {
            await _svc.RemoveAddress(addressId);
        }

        [Route("/account/address/search")]
        public async Task<IList<Address>> GetAddressesByAccountId(string accountId)
        {
            return await _svc.GetAddressesByAccountId(accountId);
        }

        [Route("/account/address/default")]
        public async Task SetDefaultAddress(string accountId, int addressId)
        {
            await _svc.SetDefultAddress(accountId, addressId);
        }

        [Route("/account/payment/{pmtId}")]
        public async Task<PaymentInfo> GetPaymentInfoById(string pmtId)
        {
            return await _svc.GetPaymentInfoById(pmtId);
        }

        [HttpPost]
        [Route("/account/payment/")]
        public async Task AddPaymentInfo([FromBody]PaymentInfo pmtInfo)
        {
            await _svc.AddPaymentInfo(pmtInfo);
        }

        [HttpPut]
        [Route("/account/payment/")]
        public async Task UpdatePaymentInfo([FromBody]PaymentInfo pmtInfo)
        {
            await _svc.UpdatePaymentInfo(pmtInfo);
        }

        [Route("/account/payment/{pmtId}")]
        [HttpDelete]
        public async Task RemovePaymentInfo(string pmtId)
        {
            await _svc.RemovePaymentInfo(pmtId);
        }

        [Route("/account/payment/search")]
        public async Task<IList<PaymentInfo>> GetPaymentInfosByAccountId(string accountId)
        {
            return await _svc.GetPaymentInfosByAccountId(accountId);
        }

        [Route("/account/payment/default")]
        public async Task SetDefaultPaymentInfo(string accountId, int pmtId)
        {
            await _svc.SetDefaultPaymentInfo(accountId, pmtId);
        }

        [Route("/account/history/{acctId}")]
        public async Task<IList<AccountHistory>> GetAccountHistory(string acctId)
        {
            return await _svc.GetAccountHistory(acctId);
        }
    }
}
