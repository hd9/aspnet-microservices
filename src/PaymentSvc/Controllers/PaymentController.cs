using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentSvc.Models;
using PaymentSvc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace PaymentSvc.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        readonly IPaymentSvc _svc;
        const string help = @"The Payment service is alive! Try GET /payments/{paymentId}.";

        public PaymentController(IPaymentSvc svc)
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

        [Route("/payments/{id}")]
        public async Task<Payment> GetPaymentById(string id)
        {
            return await _svc.GetById(id);
        }

        [Route("/payments/search")]
        public async Task<Payment> GetPaymentByAccountId(string accountId)
        {
            return await _svc.GetByAccountId(accountId);
        }
    }
}
