using MassTransit;
using Microsoft.AspNetCore.Mvc;
using OrderSvc.Models;
using OrderSvc.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderSvc.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        readonly IOrderSvc _svc;
        readonly IBusControl _bus;
        const string instruction = @"The Order service is alive! Try GET /order/{id}.";

        public OrderController(IOrderSvc svc, IBusControl bus)
        {
            _svc = svc;
            _bus = bus;
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

        [Route("/api/v1/order/{number}")]
        public async Task<IActionResult> GetOrderByNumber(string number)
        {
            var o = await _svc.GetOrderByNumber(number);
            return Ok(o);
        }

        [Route("/api/v1/orders/{accountId}")]
        public async Task<IActionResult> GetOrdersByAccountId(int accountId)
        {
            var orders = await _svc.GetOrdersByAccountId(accountId);
            return Ok(orders);
        }

        /// <summary>
        /// Accepts a submitted order from the rest api
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/v1/orders/submit")]
        public async Task<IActionResult> SubmitOrder([FromBody] Order order)
        {
            await _svc.SubmitOrder(order);
            return Ok(new { order.Number });
        }
    }
}
