using System.Collections.Generic;

namespace Web.Infrastructure.Options
{
    public class StoreSettings
    {
        public string Country { get; set; }
        public List<string> Regions { get; set; }
        public string Currency { get; set; }
        public string CurrencyDisplay { get; set; }
        public string StoreName { get; set; }
        public string StoreUrl { get; set; }
        public decimal Tax { get; set; }
    }
}
