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
        public async Task<IActionResult> Product(string pid)
        {
            var p = await _catSvc.GetProduct(pid);
            return View(p);
        }

        /// <summary>
        /// Provides the list of products from the Catalog microservice
        /// </summary>
        [Route("/api/products/categories")]
        public async Task<IList<Category>> GetCategories()
        {
            return await _catSvc.GetCategories();
        }

        /// <summary>
        /// Provides the list of products from the Catalog microservice
        /// </summary>
        [Route("/api/products/{cat}")]
        public async Task<IList<Product>> GetProducts(string cat)
        {
            return await _catSvc.GetProducts(cat);
        }

        /// <summary>
        /// Provides the list of products from the Catalog microservice
        /// </summary>
        [Route("/api/products/details/{pid}")]
        public async Task<Product> GetProductById(string pid)
        {
            return await _catSvc.GetProduct(pid);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
