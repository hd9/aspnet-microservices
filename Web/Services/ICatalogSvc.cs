using System.Collections.Generic;
using Web.Models;

namespace Web.Services
{
    public interface ICatalogSvc
    {
        List<Category> GetCategories();
        Category GetCategory(string catId);
        List<Product> GetProducts(string catId);
        List<Product> GetAllProducts();
        Product GetProduct(string pId);
    }
}