using System.Collections.Generic;
using System.Threading.Tasks;
using CatalogSvc.Models;

namespace CatalogSvc.Services
{
    public interface ICatalogSvc
    {
        Task<IList<Category>> GetCategories();
        Task<Category> GetCategory(string slug);
        Task<IList<Product>> GetProducts(string slug);
        Task<Product> GetProduct(string slug);
    }
}