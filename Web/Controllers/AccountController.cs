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
using Web.Infrastructure.Global;
using System.Net;

namespace Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAccountProxy _acctSvc;
        private readonly IOrderProxy _orderSvc;

        public AccountController(IAccountProxy accSvc, IOrderProxy oSvc, ILogger<HomeController> logger)
        {
            _acctSvc = accSvc;
            _orderSvc = oSvc;
            _logger = logger;
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
            var id = User.FindFirstValue("Id");
            var acct = await _acctSvc.GetAccountById(id);

            if (acct == null)
                return NotFound();

            return View(new AccountDetails { Name = acct.Name, Email = acct.Email });
        }

        public async Task<IActionResult> Update()
        {
            var id = User.FindFirstValue("Id");
            var acct = await _acctSvc.GetAccountById(id);

            if (acct == null)
                return NotFound();

            return View(new AccountDetails { Name = acct.Name, Email = acct.Email });
        }

        public IActionResult Addresses()
        {
            return View();
        }

        public IActionResult Payments()
        {
            return View();
        }

        public IActionResult UpdatePassword()
        {
            return View();
        }

        public IActionResult History()
        {
            return View();
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
                return View(account);
            }

            await _acctSvc.CreateAccount(account);

            TempData["Msg"] = "Account created successfully! Please login with your information below.";

            return RedirectToAction("SignIn");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Update(AccountDetails acctDetails)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMsg"] = "All fields are required. Please fill them and resubmit.";
                return View(acctDetails);
            }

            acctDetails.Id = User.FindFirstValue("Id");

            var status = await _acctSvc.UpdateAccount(acctDetails);

            if (status != HttpStatusCode.OK)
            {
                TempData["ErrorMsg"] = "Error updating your account. Please try again later.";
                return View(acctDetails);
            }

            //var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            //claimsIdentity.RemoveClaim(claimsIdentity.FindFirst("Name"));
            //claimsIdentity.AddClaim(new Claim("Name", acctDetails.Name));

            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> UpdatePassword(UpdatePassword updPassword)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMsg"] = "All fields are required. Please make sure that the new confirm password field matches the new field.";
                return View();
            }

            updPassword.AccountId = User.FindFirstValue("Id");

            await _acctSvc.UpdatePassword(updPassword);

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
