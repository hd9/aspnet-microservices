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
    public class SignupController : ControllerBase
    {

        private readonly INewsletterSvc svc;
        private readonly IConfiguration cfg;

        public SignupController(INewsletterSvc svc, IConfiguration cfg)
        {
            this.svc = svc;
            this.cfg = cfg;
        }

        [Route("/help")]
        public IActionResult Help()
        {
            var instruction = @"The service is alive! To test it, run:\ncurl -X POST ""http://<your-url>/signup"" -H 'Content-Type: application/json' -d' { ""Name"": ""tst01"", ""Email"": ""tst01 @mail.com"" }'";
            return Ok(instruction);
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
            await svc.RegistrerSignup(s);

            return Ok();
        }
        
    }
}
