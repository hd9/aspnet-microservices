using Core.Infrastructure.Options;
using NewsletterSvc.Infrastructure.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountSvc.Infrastructure.Options
{
    public class AppConfig
    {
        public string ConnectionString { get; set; } 
        public List<EmailTemplate> EmailTemplates { get; set; } 
        public MassTransitOptions MassTransit { get; set; } 
    }
}
