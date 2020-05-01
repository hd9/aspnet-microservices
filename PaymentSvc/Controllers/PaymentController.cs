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
        private readonly IPaymentSvc _svc;
        const string help = @"The service is alive!";

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

        [HttpPost]
        [Route("/payments/")]
        public async Task<IActionResult> SubmitPayment([FromBody] PaymentInfo pmt)
        {
            await _svc.SubmitPayment(pmt);
            return Ok();
        }

        //[HttpPut]
        //[Route("/payments/")]
        //public async Task<IActionResult> UpdatePayment([FromBody] PaymentInfo pmt)
        //{
        //    await _svc.UpdateAccount(account);
        //    return Ok();
        //}

        [Route("/payments/{id}")]
        public async Task<PaymentInfo> GetPaymentById(string id)
        {
            return await _svc.GetById(id);
        }

        [Route("/payments/search")]
        public async Task<PaymentInfo> GetPaymentByAccountId(string accountId)
        {
            return await _svc.GetByAccountId(accountId);
        }
    }
}
