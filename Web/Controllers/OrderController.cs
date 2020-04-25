using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Models;
using Web.Services;

namespace Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderSvc _oSvc;
        private readonly IAccountSvc _acctSvc;
        private readonly ILogger<HomeController> _logger;

        public OrderController(IOrderSvc oSvc, IAccountSvc acctSvc, ILogger<HomeController> logger)
        {
            _oSvc = oSvc;
            _acctSvc = acctSvc;
            _logger = logger;
        }

        /// <summary>
        /// My Account
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("/cart/view")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/cart/checkout")]
        public async Task<IActionResult> Checkout()
        {
            var acct = await _acctSvc.GetAccount(User.FindFirstValue("Id"));
            return View(acct);
        }

        [Route("/cart/review")]
        public async Task<IActionResult> Review()
        {
            var acct = await _acctSvc.GetAccount(User.FindFirstValue("Id"));
            return View(acct);
        }

        [HttpPost]
        public async Task<IActionResult> Submit([FromBody] Order o)
        {
            o.AccountId = User.FindFirstValue("Id");
            await _oSvc.Submit(o);
            return Ok(o.Id);
        }

        public IActionResult Submitted()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
