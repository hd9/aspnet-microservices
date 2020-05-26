using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShippingSvc.Models;
using ShippingSvc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ShippingSvc.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ShippingController : ControllerBase
    {
        private readonly IShippingSvc _svc;
        const string help = @"The Shipping service is alive! Try GET /shippings/{ShippingId}.";

        public ShippingController(IShippingSvc svc)
        {
            _svc = svc;
        }

        [Route("/ping")]
        public IActionResult Ping()
        {
            return Ok();
        }

        [Route("/help")]
        public IActionResult Help()
        {
            return Ok(help);
        }

        [Route("/Shippings/{id}")]
        public async Task<Shipping> GetShippingById(string id)
        {
            return await _svc.GetById(id);
        }

        [Route("/shippings/search")]
        public async Task<Shipping> GetShippingByAccountId(string accountId)
        {
            return await _svc.GetByAccountId(accountId);
        }
    }
}
