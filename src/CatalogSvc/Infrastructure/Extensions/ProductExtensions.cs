using CatalogSvc.Models;
using Microservices.Core.Contracts.Catalog;

namespace CatalogSvc.Infrastructure.Extensions
{
    public static class ProductExtensions
    {
        public static ProductInfo ToProductInfo(this Product product)
        {
            if (product == null)
                return null;

            return new ProductInfo
            {
                Slug = product.Slug,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            };
        }
    }
}
