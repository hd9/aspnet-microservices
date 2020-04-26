using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Services
{
    public interface ICatalogSvc
    {
        Task<List<Category>> GetCategories();
        Task<Category> GetCategory(string catId);
        Task<List<Product>> GetProducts(string catId);
        Task<Product> GetProduct(string pId);
    }
}