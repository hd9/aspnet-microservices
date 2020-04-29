using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Models;
using Web.Services;
using static Core.Infrastructure.Extentions.ExceptionExtensions;
using Core.Infrastructure.Extentions;
using Web.Infrastructure.Settings;

namespace Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAccountSvc _acctSvc;
        private readonly IOrderSvc _orderSvc;
        private readonly StoreSettings _settings;

        public AccountController(IAccountSvc accSvc, IOrderSvc oSvc, StoreSettings settings, ILogger<HomeController> logger)
        {
            _acctSvc = accSvc;
            _orderSvc = oSvc;
            _logger = logger;
            _settings = settings;
        }

        /// <summary>
        /// My Account
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Account Details
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Details()
        {
            var id = User.Claims.First(c => c.Type == "Id").Value;
            var acct = await _acctSvc.GetAccountById(id);
            ViewBag.StoreSettings = _settings;

            if (acct == null)
                return NotFound();

            return View(acct);
        }

        /// <summary>
        /// Account orders
        /// </summary>
        /// <returns></returns>
        public IActionResult Orders()
        {
            return View();
        }

        /// <summary>
        /// Allows creating an account
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult Create()
        {
            ViewBag.StoreSettings = _settings;
            return View(new Account());
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(Account account)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMsg"] = "All fields are required. Please fill them and resubmit.";
                ViewBag.StoreSettings = _settings;
                return View(account);
            }

            await _acctSvc.CreateAccount(account);

            TempData["Msg"] = "Account created successfully! Please login with your information below.";

            return RedirectToAction("SignIn");
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Update(Account account)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMsg"] = "All fields are required. Please fill them and resubmit.";
                ViewBag.StoreSettings = _settings;
                return View(account);
            }

            account.Id = User.FindFirstValue("Id");

            await _acctSvc.UpdateAccount(account);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Renders the sign in page
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult SignIn(string returnUrl)
        {
            if (returnUrl.HasValue())
                ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> SignIn(SignIn request, string returnUrl)
        {
            var acct = await _acctSvc.TrySignIn(request);

            if (acct == null)
            {
                ViewData["Error"] = "Unable to login with the information provided";
                return View();
            }
                
            var claims = new List<Claim> 
            {
                new Claim("Id", acct.Id),
                new Claim("Username", acct.Email),
                new Claim("Name", acct.Name),
                new Claim("NewsletterSubscribed", acct.SubscribedToNewsletter.ToString()),
            };

            var authProperties = new AuthenticationProperties();

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            if (returnUrl.HasValue())
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Account");
        }

        /// <summary>
        /// Signs user out
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Route("/api/account/orders")]
        public async Task<IList<Order>> GetOrders()
        {
            var acctId = User.FindFirstValue("Id");
            return await _orderSvc.GetOrdersByAccountId(acctId);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
