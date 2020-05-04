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
        public async Task<IActionResult> CreateAccount([FromBody] Account account)
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
    }
}
