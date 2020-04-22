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

        public IList<Product> GetAll()
        {
            return db.GetAll<Product>();
        }
        
    }
}
