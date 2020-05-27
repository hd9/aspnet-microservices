using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Models;
using Web.Services;

namespace Web.Controllers
{
    public class NewsletterController : Controller
    {
        readonly INewsletterProxy _svc;

        public NewsletterController(INewsletterProxy svc)
        {
            _svc = svc;
        }

        /// <summary>
        /// Receives a newsletter subscription via the Newsletter microservice on the /signup endpoint
        /// </summary>
        /// <param name="signup"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/signup")]
        public async Task<IActionResult> Signup([FromBody] NewsletterSignUp signup)
        {
            if (signup == null || !signup.IsValid())
                return BadRequest();

            await _svc.Signup(signup);

            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
