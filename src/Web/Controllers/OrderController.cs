using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Infrastructure.Global;
using Web.Models;
using Web.Models.Order;
using Web.Services;

namespace Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        readonly IOrderProxy _oSvc;
        readonly IAccountProxy _acctSvc;
        readonly ILogger<HomeController> _logger;

        public OrderController(IOrderProxy oSvc, IAccountProxy acctSvc, ILogger<HomeController> logger)
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
            var acct = await _acctSvc.GetAccountById(User.FindFirstValue("Id"));
            return View(acct);
        }

        [Route("/cart/review")]
        public async Task<IActionResult> Review(string addrId, string pmtId)
        {
            var acct = await _acctSvc.GetAccountById(User.FindFirstValue("Id"));
            var addr = await _acctSvc.GetAddressById(addrId);
            var pmt = await _acctSvc.GetPaymentInfoById(pmtId);
            
            var m = new CartReview
            {
                Account = acct,
                Address = addr,
                PaymentInfo = pmt
            };

            return View(m);
        }

        [HttpPost]
        public async Task<IActionResult> Submit([FromBody] SubmitOrder o)
        {
            int.TryParse(User.FindFirstValue("Id"), out var accId);
            o.AccountId = accId;

            o.ShippingInfo = await _acctSvc.GetAddressById(o.AddressId);
            o.PaymentInfo = await _acctSvc.GetPaymentInfoById(o.PaymentId);
            o.Currency = Site.StoreSettings.Currency;
            o.Tax = Site.StoreSettings.Tax;
            
            if (!o.IsValidForSubmit())
                return BadRequest();

            await _oSvc.Submit(o);
            return Ok(o.Number);
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
