using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NotificationSvc.Models;
using NotificationSvc.Services;

namespace NotificationSvc.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {

        readonly INotificationSvc _svc;

        public NotificationController(INotificationSvc svc)
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
            var instruction = @"The Notification service is alive! Try GET /notifications";
            return Ok(instruction);
        }

        [Route("/notifications")]
        public async Task<IActionResult> GetNotifications()
        {
            var notifs = await _svc.GetNotifications();
            return Ok(notifs);
        }
    }
}
