using Microservices.Core.Infrastructure.Options;

namespace CatalogSvc.Infrastructure.Options
{
    public class AppConfig
    {
        public MassTransitOptions MassTransit { get; set; } 
        public MongoOptions Mongo { get; set; } 
    }
}
