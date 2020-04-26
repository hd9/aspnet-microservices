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
        [Route("/products/{slug}")]
        public async Task<IActionResult> Products(string slug)
        {
            var cat = await _catSvc.GetCategory(slug);
            return View(cat);
        }

        /// <summary>
        /// Provides the list of products from the Catalog microservice
        /// </summary>
        [Route("/product/{slug}")]
        public async Task<IActionResult> Product(string slug)
        {
            var p = await _catSvc.GetProductBySlug(slug);
            return View(p);
        }

        /// <summary>
        /// Provides the list of products from the Catalog microservice
        /// </summary>
        [Route("/api/categories")]
        public async Task<IList<Category>> GetCategories()
        {
            return await _catSvc.GetCategories();
        }

        /// <summary>
        /// Provides the list of products from the Catalog microservice
        /// </summary>
        [Route("/api/products/{slug}")]
        public async Task<IList<Product>> GetProductsByCategory(string slug)
        {
            return await _catSvc.GetProductsByCategory(slug);
        }

        /// <summary>
        /// Provides the list of products from the Catalog microservice
        /// </summary>
        [Route("/api/product/{slug}")]
        public async Task<Product> GetProductBySlug(string slug)
        {
            return await _catSvc.GetProductBySlug(slug);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
