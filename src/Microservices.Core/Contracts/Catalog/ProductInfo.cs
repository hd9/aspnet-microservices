namespace Microservices.Core.Contracts.Catalog
{
    public class ProductInfo
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
