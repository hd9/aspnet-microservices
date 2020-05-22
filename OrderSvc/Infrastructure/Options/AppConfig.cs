using HildenCo.Core.Infrastructure.Options;

namespace OrderSvc.Infrastructure.Options
{
    public class AppConfig
    {
        public MassTransitOptions MassTransit { get; set; } 
        public string ConnectionString { get; set; } 
    }
}
