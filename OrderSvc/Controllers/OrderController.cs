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

        [Route("/order/{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var orders = await _svc.GetOrderById(id);
            return Ok(orders);
        }

        [Route("/orders/{accountId}")]
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
        [Route("/orders/submit")]
        public async Task<IActionResult> SubmitOrder([FromBody] Order order)
        {
            await _svc.SubmitOrder(order);
            return Ok(order.Id);
        }
    }
}
