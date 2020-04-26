using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatalogSvc.Models;
using CatalogSvc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CatalogSvc.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogSvc svc;
        private readonly IConfiguration cfg;
        const string instruction = @"The service is alive! To test it, run: curl ""http://<your-url>/products/all""";

        public CatalogController(ICatalogSvc svc, IConfiguration cfg)
        {
            this.svc = svc;
            this.cfg = cfg;
        }

        /// <summary>
        /// Settings: Returns the connection string. Use for debugging purposes.
        /// </summary>
        /// <returns></returns>
        public IActionResult Settings()
        {
            // Note: the configuration manager loads configuration based on the following criteria:
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.1#default-configuration
            // In our container, we'll provide them via env vars 
            var cs = cfg["DbSettings:ConnStr"];
            var db = cfg["DbSettings:Db"];
            var c = cfg["DbSettings:Collection"];

            return Ok($"Connection String: {cs}, Db: {db}, Collection: {c}");
        }

        [Route("/ping")]
        public IActionResult Ping()
        {
            return Ok();
        }

        [Route("/help")]
        public IActionResult Help()
        {
            return Ok(instruction);
        }

        [HttpGet]
        [Route("/product/{slug}")]
        public async Task<Product> GetProductBySlug(string slug)
        {
            return await svc.GetProduct(slug);
        }

        [HttpGet]
        [Route("/products/{slug}")]
        public async Task<IList<Product>> GetProductsByCategory(string slug)
        {
            return await svc.GetProducts(slug);
        }

        [HttpGet]
        [Route("/categories")]
        public async Task<IList<Category>> GetCategories()
        {
            return await svc.GetCategories();
        }

        [HttpGet]
        [Route("/categories/{slug}")]
        public async Task<Category> GetCategory(string slug)
        {
            return await svc.GetCategory(slug);
        }
    }
}
