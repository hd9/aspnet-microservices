using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Infrastructure.Options
{
    public class MassTransitOptions
    {
        public string Host { get; set; }
        public string Queue { get; set; }
    }
}
