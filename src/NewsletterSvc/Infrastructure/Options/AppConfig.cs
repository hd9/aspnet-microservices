using Microservices.Core.Infrastructure.Options;

namespace NewsletterSvc.Infrastructure.Options
{
    public class AppConfig
    {
        public string ConnectionString { get; set; }
        public MassTransitOptions MassTransit { get; set; } 
        public EmailTemplate EmailTemplate { get; set; } 
    }
}
