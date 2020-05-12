using Core.Infrastructure.Extentions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Models;
using Web.Services;

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

        /// <summary>
        /// Displays all addresses under account
        /// </summary>
        /// <returns></returns>
        public IActionResult Address()
        {
            return View();
        }

        /// <summary>
        /// Adds a new address to the account
        /// </summary>
        /// <returns></returns>
        [Route("/account/address/add")]
        public IActionResult AddAddress()
        {
            return View(new Address());
        }

        /// <summary>
        /// Allows editing an address
        /// </summary>
        /// <param name="addrId"></param>
        /// <returns></returns>
        [Route("/account/address/edit/{addrId}")]
        public async Task<IActionResult> EditAddress(string addrId)
        {
            var addr = await _acctSvc.GetAddressById(addrId);
            return View(addr);
        }

        /// <summary>
        /// Manage payment methods
        /// </summary>
        /// <returns></returns>
        public IActionResult Payments()
        {
            return View();
        }

        /// <summary>
        /// Adds a new payment method to the account
        /// </summary>
        /// <returns></returns>
        [Route("/account/payment/add")]
        public IActionResult AddPayment()
        {
            return View(new PaymentInfo());
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Route("/account/payment/add")]
        public async Task<IActionResult> AddPayment(PaymentInfo pmtInfo)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMsg"] = "All fields are required. Please fill them and resubmit.";
                return View(pmtInfo);
            }

            int.TryParse(User.FindFirstValue("Id"), out var accId);
            pmtInfo.AccountId = accId;
            var resp = await _acctSvc.AddPayment(pmtInfo);

            if (resp != HttpStatusCode.OK)
            {
                TempData["ErrorMsg"] = "Error adding your payment, please try again later.";
                return View();
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Updates an existing address
        /// </summary>
        /// <param name="addr"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> UpdatePayment(PaymentInfo pmtInfo)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMsg"] = "Please make sure you filled all fields.";
                return View();
            }

            int.TryParse(User.FindFirstValue("Id"), out var accId);
            pmtInfo.AccountId = accId;
            var resp = await _acctSvc.UpdatePayment(pmtInfo);

            if (resp != HttpStatusCode.OK)
            {
                TempData["ErrorMsg"] = "Error updating your payment information, please try again later.";
                return View();
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Allows editing a payment method
        /// </summary>
        /// <param name="addrId"></param>
        /// <returns></returns>
        [Route("/account/payment/edit/{pmtId}")]
        public async Task<IActionResult> EditPayment(string pmtId)
        {
            var pmtInfo = await _acctSvc.GetPaymentInfoById(pmtId);
            return View(pmtInfo);
        }

        /// <summary>
        /// Update password
        /// </summary>
        /// <returns></returns>
        public IActionResult UpdatePassword()
        {
            return View();
        }

        /// <summary>
        /// Account history
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Creates a new account. Public endpoint
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Update account information
        /// </summary>
        /// <param name="acctDetails"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Updates the account's password
        /// </summary>
        /// <param name="updPassword"></param>
        /// <returns></returns>
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
        /// Adds a new address to the account
        /// </summary>
        /// <param name="addr"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        [Route("/account/address/add")]
        public async Task<IActionResult> AddAddress(Address addr)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMsg"] = "All fields are required. Please fill them and resubmit.";
                return View(addr);
            }

            addr.AccountId = int.Parse(User.FindFirstValue("Id"));
            var resp = await _acctSvc.AddAddress(addr);

            if (resp != HttpStatusCode.OK)
            {
                TempData["ErrorMsg"] = "Error adding your address, please try again later.";
                return View();
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Updates an existing address
        /// </summary>
        /// <param name="addr"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> UpdateAddress(Address addr)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMsg"] = "Please make sure you filled all fields.";
                return View();
            }

            addr.AccountId = int.Parse(User.FindFirstValue("Id"));
            var resp = await _acctSvc.UpdateAddress(addr);

            if (resp != HttpStatusCode.OK)
            {
                TempData["ErrorMsg"] = "Error updating your address, please try again later.";
                return View();
            }

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

        [Route("/api/account/addresses")]
        public async Task<IList<Address>> GetAddresses()
        {
            var acctId = User.FindFirstValue("Id");
            return await _acctSvc.GetAddressesByAccountId(acctId);
        }

        [HttpDelete]
        [Route("/api/account/address/{addressId}")]
        public async Task<IActionResult> RemoveAddress(string addressId)
        {
            var resp = await _acctSvc.RemoveAddress(addressId);

            if (resp == HttpStatusCode.OK)
                return Ok();
                    
            return BadRequest();
        }

        [HttpPut]
        [Route("/api/account/address/{addressId}")]
        public async Task<IActionResult> SetDefaultAddress(int addressId)
        {
            var acctId = User.FindFirstValue("Id");
            var resp = await _acctSvc.SetDefaultAddress(acctId, addressId);

            if (resp == HttpStatusCode.OK)
                return Ok();

            return BadRequest();
        }

        [Route("/api/account/payments")]
        public async Task<IList<PaymentInfo>> GetPayments()
        {
            var acctId = User.FindFirstValue("Id");
            return await _acctSvc.GetPaymentInfosByAccountId(acctId);
        }

        [HttpDelete]
        [Route("/api/account/payment/{pmtId}")]
        public async Task<IActionResult> RemovePayment(string pmtId)
        {
            var resp = await _acctSvc.RemovePayment(pmtId);

            if (resp == HttpStatusCode.OK)
                return Ok();

            return BadRequest();
        }

        [HttpPut]
        [Route("/api/account/payment/{pmtId}")]
        public async Task<IActionResult> SetDefaultPayment(int pmtId)
        {
            var acctId = User.FindFirstValue("Id");
            var resp = await _acctSvc.SetDefaultPayment(acctId, pmtId);

            if (resp == HttpStatusCode.OK)
                return Ok();

            return BadRequest();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
