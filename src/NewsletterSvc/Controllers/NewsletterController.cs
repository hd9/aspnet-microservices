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

        readonly INewsletterSvc _svc;

        public NewsletterController(INewsletterSvc svc)
        {
            _svc = svc;
        }

        [Route("/help")]
        public IActionResult Help()
        {
            return Ok("The Newsletter service is alive! Try GET /signups");
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
            if (s == null)
                return BadRequest();

            await _svc.RegistrerSignup(s);

            return Ok();
        }

        [Route("/signups")]
        public async Task<IActionResult> GetSignups()
        {
            var signups = await _svc.GetSignups();
            return Ok(signups);
        }
    }
}
