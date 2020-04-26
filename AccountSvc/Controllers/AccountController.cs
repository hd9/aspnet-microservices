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
        const string instruction = @"The service is alive!";

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
            return Ok(instruction);
        }

    }
}
