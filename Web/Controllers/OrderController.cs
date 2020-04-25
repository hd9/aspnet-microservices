using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Web.Models;
using Web.Services;

namespace Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderSvc _oSvc;
        private readonly ILogger<HomeController> _logger;

        public OrderController(IOrderSvc oSvc, ILogger<HomeController> logger)
        {
            _oSvc = oSvc;
            _logger = logger;
        }

        /// <summary>
        /// My Account
        /// </summary>
        /// <returns></returns>
        [Route("/cart/view")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/cart/checkout")]
        public IActionResult Checkout()
        {
            // todo :: load acct
            var a = new Account { Id = "123", Email = "m@ma.com", Name = "Acct Name", Address = "addr 123 345", City = "city", Country = "Canada" };
            return View(a);
        }

        [Route("/cart/review")]
        public IActionResult Review()
        {
            // todo :: load acct
            var a = new Account { Id = "123", Email = "m@ma.com", Name = "Acct Name", Address = "addr 123 345", City = "city", Country = "Canada" };
            return View(a);
        }

        [HttpPost]
        public async Task<IActionResult> Submit([FromBody] Order o)
        {
            // todo :: order svc
            o.Id = Guid.NewGuid().ToString();
            await _oSvc.Submit(o);
            return Ok(o.Id);
        }


        public IActionResult Submitted()
        {
            // todo :: load acct
            var o = new Order { Id = "O-123x890", CreatedOn = DateTime.UtcNow, LineItems = new List<LineItem> {
                new LineItem { Id = "l-12", Name = "PS4", Price = 400f, Qty = 1 } }
            };

            return View(o);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
