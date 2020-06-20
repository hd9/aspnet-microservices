using Microservices.Core.Infrastructure.Options;

namespace PaymentSvc.Infrastructure.Options
{
    public class AppConfig
    {
        public MassTransitOptions MassTransit { get; set; }
        public string ConnectionString { get; set; } 
    }
}
