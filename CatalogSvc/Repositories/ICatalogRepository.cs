using CatalogSvc.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CatalogSvc.Repositories
{
    public interface ICatalogRepository
    {
        Task<IList<Category>> GetCategories();
        Task<Category> GetCategory(string slug);
        Task<Product> GetProduct(string slug);
        Task<IList<Product>> GetProductsByCategory(string slug);
        Task<IList<Product>> GetProducts(List<string> slugs);
    }
}