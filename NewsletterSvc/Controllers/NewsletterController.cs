using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NewsletterSvc.Models;
using NewsletterSvc.Services;

namespace NewsletterSvc.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class NewsletterController : ControllerBase
    {

        private readonly INewsletterSvc _svc;

        public NewsletterController(INewsletterSvc svc)
        {
            _svc = svc;
        }

        [Route("/help")]
        public IActionResult Help()
        {
            var instruction = @"The service is alive! To test it, run:\ncurl -X POST ""http://<your-url>/signup"" -H 'Content-Type: application/json' -d' { ""Name"": ""tst01"", ""Email"": ""tst01 @mail.com"" }'";
            return Ok(instruction);
        }

        [Route("/ping")]
        public IActionResult Ping()
        {
            return Ok();
        }

        /// <summary>
        /// Creates a new signup request
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        [Route("/signup")]
        [HttpPost]
        public async Task<IActionResult> Signup(Signup s)
        {
            if (s == null) return BadRequest();
            await _svc.RegistrerSignup(s);

            return Ok();
        }
        
    }
}
