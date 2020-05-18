using Core.Infrastructure.Options;
using System;
using System.Text;

namespace CatalogSvc.Infrastructure.Options
{
    public class AppConfig
    {
        public MassTransitOptions MassTransit { get; set; } 
        public MongoOptions Mongo { get; set; } 
    }
}
