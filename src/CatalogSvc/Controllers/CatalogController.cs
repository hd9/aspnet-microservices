using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatalogSvc.Models;
using CatalogSvc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microservices.Core.Infrastructure.Extensions;

namespace CatalogSvc.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        readonly ICatalogSvc svc;
        const string instruction = "The Catalog service is alive! Try GET /api/v1/categories";

        public CatalogController(ICatalogSvc svc)
        {
            this.svc = svc;
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
        [Route("/api/v1/product/{slug}")]
        public async Task<Product> GetProductBySlug(string slug)
        {
            return await svc.GetProduct(slug);
        }

        [HttpGet]
        [Route("/api/v1/products/search")]
        public async Task<IActionResult> GetProductBySlugs(string slugs)
        {
            var lst = (slugs ?? "").Split(
                ",",
                StringSplitOptions.RemoveEmptyEntries
            ).ToList();

            if (!lst.HasAny())
                return Ok();

            return Ok(await svc.GetProducts(lst));
        }

        [HttpGet]
        [Route("/api/v1/products/{slug}")]
        public async Task<IList<Product>> GetProductsByCategory(string slug)
        {
            return await svc.GetProductsByCategory(slug);
        }

        [HttpGet]
        [Route("/api/v1/categories")]
        public async Task<IList<Category>> GetCategories()
        {
            return await svc.GetCategories();
        }

        [HttpGet]
        [Route("/api/v1/categories/{slug}")]
        public async Task<Category> GetCategory(string slug)
        {
            return await svc.GetCategory(slug);
        }
    }
}
