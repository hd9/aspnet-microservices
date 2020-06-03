using Microservices.Core.Infrastructure.Options;
using System.Collections.Generic;

namespace AccountSvc.Infrastructure.Options
{
    public class AppConfig
    {
        public string ConnectionString { get; set; } 
        public List<EmailTemplate> EmailTemplates { get; set; } 
        public MassTransitOptions MassTransit { get; set; } 
    }
}
