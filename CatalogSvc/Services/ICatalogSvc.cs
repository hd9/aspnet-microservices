using System.Collections.Generic;
using CatalogSvc.Models;

namespace CatalogSvc.Services
{
    public interface ICatalogSvc
    {
        IList<Product> GetAll();
    }
}