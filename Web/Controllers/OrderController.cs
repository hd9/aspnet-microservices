using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Web.Models;

namespace Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public OrderController(ILogger<HomeController> logger)
        {
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
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
