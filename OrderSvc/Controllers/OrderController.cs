using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderSvc.Models;
using OrderSvc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace OrderSvc.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderSvc _svc;
        private readonly IConfiguration _cfg;
        const string instruction = @"The Order service is alive! Try GET /orders/{accountId}.";

        public OrderController(IOrderSvc svc, IConfiguration cfg)
        {
            _svc = svc;
            _cfg = cfg;
        }

        [Route("/ping")]
        public IActionResult Ping()
        {
            return Ok();
        }

        [Route("/help")]
        public IActionResult Help()
        {
            return Ok(instruction);
        }

        [Route("/orders/{accountId}")]
        public async Task<IActionResult> GetOrdersByAccountId(int accountId)
        {
            var orders = await _svc.GetOrdersByAccountId(accountId);
            return Ok(orders);
        }

        [HttpPost]
        [Route("/orders/submit")]
        public async Task<IActionResult> SubmitOrder([FromBody] Order order)
        {
            var orderId = await _svc.SubmitOrder(order);
            return Ok(orderId);
        }
    }
}
