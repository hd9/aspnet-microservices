using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Models;
using Web.Services;

namespace Web.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ILogger<ApiController> _logger;
        private readonly ICatalogSvc _catSvc;
        public CatalogController(ICatalogSvc catSvc, ILogger<ApiController> logger)
        {
            _catSvc = catSvc;
            _logger = logger;
        }

        /// <summary>
        /// Provides the list of products from the Catalog microservice
        /// </summary>
        [Route("/products/{catId}")]
        public IActionResult Products(string catId)
        {
            var cat = _catSvc.GetCategory(catId);
            return View(cat);
        }

        /// <summary>
        /// Provides the list of products from the Catalog microservice
        /// </summary>
        [Route("/products/details/{pid}")]
        public IActionResult Product(string pid)
        {
            var p = _catSvc.GetProduct(pid);
            return View(p);
        }

        /// <summary>
        /// Provides the list of products from the Catalog microservice
        /// </summary>
        [Route("/api/products/categories")]
        public IList<Category> GetCategories()
        {
            return _catSvc.GetCategories();
        }

        /// <summary>
        /// Provides the list of products from the Catalog microservice
        /// </summary>
        [Route("/api/products/")]
        public IList<Product> GetAllProducts()
        {
            return _catSvc.GetAllProducts();
        }

        /// <summary>
        /// Provides the list of products from the Catalog microservice
        /// </summary>
        [Route("/api/products/{cat}")]
        public IList<Product> GetProducts(string cat)
        {
            return _catSvc.GetProducts(cat);
        }

        /// <summary>
        /// Provides the list of products from the Catalog microservice
        /// </summary>
        [Route("/api/products/details/{pid}")]
        public Product GetProductById(string pid)
        {
            return _catSvc.GetProduct(pid);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
