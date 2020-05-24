using HildenCo.Core.Infrastructure.Options;
using System.Collections.Generic;

namespace OrderSvc.Infrastructure.Options
{
    public class AppConfig
    {
        public MassTransitOptions MassTransit { get; set; } 
        public List<EmailTemplate> EmailTemplates { get; set; } 
        public string ConnectionString { get; set; }
    }
}
