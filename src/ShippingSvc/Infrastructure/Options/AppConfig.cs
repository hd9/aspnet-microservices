﻿using Microservices.Core.Infrastructure.Options;

namespace ShippingSvc.Infrastructure.Options
{
    public class AppConfig
    {
        public MassTransitOptions MassTransit { get; set; }
        public string ConnectionString { get; set; } 
    }
}
