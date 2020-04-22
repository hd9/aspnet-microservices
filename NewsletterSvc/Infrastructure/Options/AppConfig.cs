using Core.Infrastructure.Options;
using System;

namespace NewsletterSvc.Infrastructure.Options
{
    public class AppConfig
    {
        public MongoOptions MongoDb { get; set; } 
        public MassTransitOptions MassTransit { get; set; } 
    }
}
