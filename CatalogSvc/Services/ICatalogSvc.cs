using System.Collections.Generic;
using System.Threading.Tasks;
using CatalogSvc.Models;

namespace CatalogSvc.Services
{
    public interface ICatalogSvc
    {
        Task<IList<Category>> GetCategories();
        Task<Category> GetCategory(string categoryId);
        Task<IList<Product>> GetProducts(string categoryId);
        Task<Product> GetProduct(string productId);
    }
}