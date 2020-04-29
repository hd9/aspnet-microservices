using Core.Infrastructure.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Infrastructure.Settings
{
    public class StoreSettings
    {
        public string Country { get; set; }
        public List<string> Regions { get; set; }
        public string Currency { get; set; }
        public decimal Tax { get; set; }
    }
}
