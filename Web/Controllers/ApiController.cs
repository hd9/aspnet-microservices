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
    public class ApiController : Controller
    {
        private readonly ILogger<ApiController> _logger;
        private readonly INewsletterProxy _nlSvc;
        public ApiController(INewsletterProxy nlSvc, ILogger<ApiController> logger)
        {
            _nlSvc = nlSvc;
            _logger = logger;
        }

        /// <summary>
        /// Receives a newsletter subscription via the Newsletter microservice on the /signup endpoint
        /// </summary>
        /// <param name="signup"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Signup([FromBody] NewsletterSignUp signup)
        {
            if (signup == null) return BadRequest();
            _nlSvc.Signup(signup);

            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
