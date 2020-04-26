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
        private readonly IOrderSvc svc;
        private readonly IConfiguration cfg;
        const string instruction = @"The service is alive!";

        public OrderController(IOrderSvc svc, IConfiguration cfg)
        {
            this.svc = svc;
            this.cfg = cfg;
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

    }
}
