using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Services
{
    public interface ICatalogSvc
    {
        Task<List<Category>> GetCategories();
        Task<Category> GetCategory(string slug);
        Task<List<Product>> GetProductsByCategory(string slug);
        Task<Product> GetProductBySlug(string slug);
    }
}