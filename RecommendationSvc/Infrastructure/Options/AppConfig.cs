﻿using Core.Infrastructure.Options;
using System;
using System.Text;

namespace RecommendationSvc.Infrastructure.Options
{
    public class AppConfig
    {
        public MassTransitOptions MassTransit { get; set; } 
        public string ConnectionString { get; set; } 
    }
}
