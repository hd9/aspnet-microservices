using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Models;
using Web.Services;

namespace Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAccountSvc _accSvc;

        public AccountController(IAccountSvc accSvc, ILogger<HomeController> logger)
        {
            _accSvc = accSvc;
            _logger = logger;
        }

        /// <summary>
        /// My Account
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            // todo :: get acct from session
            return View();
        }

        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult SignIn()
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
