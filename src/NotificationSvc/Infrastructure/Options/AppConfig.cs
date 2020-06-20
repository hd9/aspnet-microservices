using Microservices.Core.Infrastructure.Options;

namespace NotificationSvc.Infrastructure.Options
{
    public class AppConfig
    {
        public MassTransitOptions MassTransit { get; set; }
        public string ConnectionString { get; set; } 
        public SmtpOptions SmtpOptions { get; set; } 
    }
}
