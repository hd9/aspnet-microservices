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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Models;
using Web.Services;
using static Core.Infrastructure.Extentions.ExceptionExtensions;
using Core.Infrastructure.Extentions;

namespace Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAccountSvc _acctSvc;
        private readonly IOrderSvc _oSvc;

        public AccountController(IAccountSvc accSvc, IOrderSvc oSvc, ILogger<HomeController> logger)
        {
            _acctSvc = accSvc;
            _oSvc = oSvc;
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
            var id = User.Claims.First(c => c.Type == "Id").Value;
            var acct = await _acctSvc.GetAccount(id);

            Throw<UnauthorizedAccessException>.If(acct == null);

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
            return View(new Account());
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(Account request)
        {
            throw new NotImplementedException();
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Update(Account request)
        {
            if (request == null)
                return BadRequest();

            request.Id = User.FindFirstValue("Id");

            await _acctSvc.UpdateAccount(request);
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
            return await _oSvc.GetOrders(acctId);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
