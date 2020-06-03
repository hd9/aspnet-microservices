using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Core.Contracts.Catalog
{
    public class ProductInfoRequest
    {
        public List<string> Slugs { get; set; }
    }
}
