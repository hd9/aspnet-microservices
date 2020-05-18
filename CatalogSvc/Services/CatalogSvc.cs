using CatalogSvc.Models;
using CatalogSvc.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CatalogSvc.Services
{
    public class CatalogSvc : ICatalogSvc
    {
        private readonly ICatalogRepository _repo;

        public CatalogSvc(ICatalogRepository repo)
        {
            _repo = repo;
        }

        public async Task<IList<Category>> GetCategories()
        {
            return await _repo.GetCategories();
        }

        public async Task<Category> GetCategory(string slug)
        {
            return await _repo.GetCategory(slug);
        }

        public async Task<Product> GetProduct(string slug)
        {
            return await _repo.GetProduct(slug);
        }

        public async Task<IList<Product>> GetProducts(List<string> slugs)
        {
            return await _repo.GetProducts(slugs);
        }

        public async Task<IList<Product>> GetProductsByCategory(string slug)
        {
            return await _repo.GetProductsByCategory(slug);
        }
    }
}
