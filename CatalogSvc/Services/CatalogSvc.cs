using CatalogSvc.Infrastructure;
using CatalogSvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogSvc.Services
{
    public class CatalogSvc : ICatalogSvc
    {
        private readonly IMongoClient db;

        public CatalogSvc(IMongoClient db)
        {
            this.db = db;
        }

        public async Task<IList<Category>> GetCategories()
        {
            db.Collection = "Categories";
            return await db.GetAll<Category>();
        }

        public async Task<Category> GetCategory(string categoryId)
        {
            db.Collection = "Categories";
            return (await db.Find<Category>("Slug", categoryId)).SingleOrDefault();
        }

        public async Task<Product> GetProduct(string productId)
        {
            return (await db.Find<Product>("Slug", productId)).SingleOrDefault();
        }

        public async Task<IList<Product>> GetProducts(string categoryId)
        {
            return await db.Find<Product>("CategoryId", categoryId);
        }

    }
}
