﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Models;
using Web.Models.Catalog;

namespace Web.Services
{
    public interface ICatalogProxy
    {
        Task<List<Category>> GetCategories();
        Task<Category> GetCategory(string slug);
        Task<List<Product>> GetProductsByCategory(string slug);
        Task<Product> GetProductBySlug(string slug);
    }
}