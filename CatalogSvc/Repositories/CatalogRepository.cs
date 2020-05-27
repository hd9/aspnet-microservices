using CatalogSvc.Infrastructure.Db;
using CatalogSvc.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogSvc.Repositories
{
    public class CatalogRepository : ICatalogRepository
    {
        readonly IMongoClient db;

        public CatalogRepository(IMongoClient db)
        {
            this.db = db;
        }

        public async Task<IList<Category>> GetCategories()
        {
            db.Collection = "categories";
            return await db.GetAll<Category>();
        }

        public async Task<Category> GetCategory(string slug)
        {
            db.Collection = "categories";
            return (await db.Find<Category>("Slug", slug)).SingleOrDefault();
        }

        public async Task<Product> GetProduct(string slug)
        {
            db.Collection = "products";
            return (await db.Find<Product>("Slug", slug)).SingleOrDefault();
        }

        public async Task<IList<Product>> GetProducts(List<string> slugs)
        {
            db.Collection = "products";
            return await db.Find<Product>("Slug", slugs);
        }

        public async Task<IList<Product>> GetProductsByCategory(string slug)
        {
            db.Collection = "products";
            return await db.Find<Product>("CategoryId", slug);
        }

    }
}
