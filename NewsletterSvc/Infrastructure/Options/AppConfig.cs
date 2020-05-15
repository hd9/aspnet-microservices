using Core.Infrastructure.Options;
using System;

namespace NewsletterSvc.Infrastructure.Options
{
    public class AppConfig
    {
        public string ConnectionString { get; set; }
        public MassTransitOptions MassTransit { get; set; } 
        public EmailTemplate EmailTemplate { get; set; } 
    }
}
