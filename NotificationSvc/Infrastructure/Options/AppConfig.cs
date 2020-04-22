using Core.Infrastructure.Options;
using System;
using System.Text;

namespace NotificationSvc.Infrastructure.Options
{
    public class AppConfig
    {
        public MailOptions Mail { get; set; } 
        public MassTransitOptions MassTransit { get; set; } 
        public string ConnectionString { get; set; } 
    }
}
