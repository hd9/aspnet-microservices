using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Commands.Catalog
{
    public class ProductInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }

    public class ProductInfoResponse
    {
        public List<ProductInfo> ProductInfos { get; set; }
    }
}
