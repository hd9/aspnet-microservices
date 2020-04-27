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
        private readonly IAccountSvc svc;
        private readonly IConfiguration cfg;
        const string help = @"The service is alive!";

        public AccountController(IAccountSvc svc, IConfiguration cfg)
        {
            this.svc = svc;
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

        [Route("/test")]
        public async Task<IActionResult> Test()
        {
            var a = await svc.GetAccountById("1");
            return Ok(a);
        }

        [HttpPost]
        [Route("/account/")]
        public async Task<IActionResult> CreateAccount([FromBody] Account account)
        {
            await svc.CreateAccount(account);
            return Ok();
        }

        [HttpPut]
        [Route("/account/")]
        public async Task<IActionResult> UpdateAccount([FromBody] Account account)
        {
            await svc.UpdateAccount(account);
            return Ok();
        }

        [Route("/account/{id}")]
        public async Task<Account> GetAccount(string id)
        {
            return await svc.GetAccountById(id);
        }

        [Route("/account/search")]
        public async Task<Account> GetAccountByEmail(string email)
        {
            return await svc.GetAccountByEmail(email);
        }
    }
}
