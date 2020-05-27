using HildenCo.Core.Infrastructure.Options;
using System.Collections.Generic;

namespace Web.Infrastructure.Options
{
    public class AppConfig
    {
        public StoreSettings StoreSettings { get; set; } 
        public ServiceConfig Services { get; set; } 
        public RedisOptions Redis { get; set; } 
    }
}
