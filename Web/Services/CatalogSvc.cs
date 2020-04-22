using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Web.Models;

namespace Web.Services
{
    public class CatalogSvc : ICatalogSvc
    {
        private readonly ILogger<CatalogSvc> logger;
        private readonly HttpClient httpClient;
        private readonly IConfiguration cfg;
        private readonly List<Product> products;
        private const string lorem1 = "Nesciunt et dolorum porro soluta et officiis. Fugit ut minima repudiandae aut qui corporis omnis necessitatibus.";
        private const string lorem2 = "Pariatur quidem delectus facere est qui. Quia ea earum nesciunt aliquam veritatis tempora.";
        private const string lorem3 = "Consequatur iure suscipit nemo repellendus. Ipsa minus et ea illum quis.";

        public CatalogSvc(HttpClient httpClient, IConfiguration cfg, ILogger<CatalogSvc> logger)
        {
            this.logger = logger;
            this.httpClient = httpClient;
            this.cfg = cfg;

            products = new List<Product>
            {
                new Product { Id = "xbx-123", Name = "Xbox One", Price = 300, Currency = "CAD", Description = "TODO", CategoryId = "games", CategoryName = "Games", Rating = 5 } ,
                new Product { Id = "ps4-456", Name = "PS4", Price = 300, Currency = "CAD", Description = "TODO", CategoryId = "games", CategoryName = "Games", Rating = 5 } ,
                new Product { Id = "ns-789", Name = "Nintendo Switch", Price = 300, Currency = "CAD", Description = "TODO", CategoryId = "games", CategoryName = "Games", Rating = 5 } ,
                new Product { Id = "ps5-753;", Name = "PS5", Price = 300, Currency = "CAD", Description = "TODO", CategoryId = "games", CategoryName = "Games", Rating = 4 } ,
                new Product { Id = "xbxx-951", Name = "Xbox X Series", Price = 300, Currency = "CAD", Description = "TODO", CategoryId = "games", CategoryName = "Games", Rating = 4 } ,
                new Product { Id = "wiiu-789", Name = "wii U", Price = 300, Currency = "CAD", Description = "TODO", CategoryId = "games", CategoryName = "Games", Rating = 5 } ,
                new Product { Id = "fdr-951", Name = "Fender Stratocaster", Price = 800, Currency = "CAD", Description = "TODO", CategoryId = "musical-instruments", CategoryName = "Musical Instruments", Rating = 5 } ,
                new Product { Id = "gib-789", Name = "Gibson Les Paul", Price = 1500, Currency = "CAD", Description = "TODO", CategoryId = "musical-instruments", CategoryName = "Musical Instruments", Rating = 3 } ,
                new Product { Id = "gljb-789", Name = "Geddy Lee's Jazz Bass", Price = 1500, Currency = "CAD", Description = "TODO", CategoryId = "musical-instruments", CategoryName = "Musical Instruments", Rating = 4 } ,
                new Product { Id = "pb-951", Name = "Paddle Boarding", Price = 500, Currency = "CAD", Description = "TODO", CategoryId = "sports", CategoryName = "Sports", Rating = 3 } ,
                new Product { Id = "jjk-789", Name = "Jiu-Jitsu Kimono", Price = 150, Currency = "CAD", Description = "TODO", CategoryId = "sports", CategoryName = "Sports", Rating = 3 } ,
                new Product { Id = "sb-789", Name = "Soccer Ball", Price = 15, Currency = "CAD", Description = "TODO", CategoryId = "sports", CategoryName = "Sports", Rating = 3 } ,
                new Product { Id = "goia-789", Name = "Go in Action", Price = 45, Currency = "CAD", Description = "TODO", CategoryId = "books", CategoryName = "Books", Rating = 3 } ,
                new Product { Id = "goia-789", Name = "Algorithms", Price = 45, Currency = "CAD", Description = " by Robert Sedgewick & Kevin Wayne TODO", CategoryId = "books", CategoryName = "Books", Rating = 3 } ,
                new Product { Id = "cd-789", Name = "Continuous Delivery", Price = 45, Currency = "CAD", Description = "by Jez Humble & David Farley", CategoryId = "books", CategoryName = "Books", Rating = 3 } ,
                new Product { Id = "rpi-789", Name = "Raspberry Pi", Price = 45, Currency = "CAD", Description = "TODO", CategoryId = "computers", CategoryName = "Computers", Rating = 3 } ,
                new Product { Id = "pppro-789", Name = "Pine Book Pro ARM", Price = 45, Currency = "CAD", Description = "TODO", CategoryId = "computers", CategoryName = "Computers", Rating = 3 } ,
                new Product { Id = "pppro-789", Name = "Sony Bravia 55", Price = 450, Currency = "CAD", Description = "TODO", CategoryId = "tvs", CategoryName = "TVs", Rating = 4 } ,
                new Product { Id = "pppro-789", Name = "Toshiba Netflix 55", Price = 450, Currency = "CAD", Description = "TODO", CategoryId = "tvs", CategoryName = "TVs", Rating = 4 } ,
                new Product { Id = "ggc-789", Name = "Grass Curter", Price = 40, Currency = "CAD", Description = "TODO", CategoryId = "home", CategoryName = "Home & Garden", Rating = 4 } ,
                new Product { Id = "bbqc-789", Name = "BBQ Coal", Price = 40, Currency = "CAD", Description = "TODO", CategoryId = "home", CategoryName = "Home & Garden", Rating = 4 } ,
                new Product { Id = "seab-789", Name = "Sony Earbuddy", Price = 40, Currency = "CAD", Description = "TODO", CategoryId = "headphones-audio", CategoryName = "Headphones & Audio", Rating = 4 },
                new Product { Id = "iphone-753", Name = "iPhone 7", Price = 400, Currency = "CAD", Description = "TODO", CategoryId = "phones", CategoryName = "Headphones & Audio", Rating = 4 },
                new Product { Id = "iphone-X11", Name = "iPhone X", Price = 999, Currency = "CAD", Description = "TODO", CategoryId = "phones", CategoryName = "Headphones & Audio", Rating = 4 },
                new Product { Id = "sg-11", Name = "Samsung Galaxy 11", Price = 600, Currency = "CAD", Description = "TODO", CategoryId = "phones", CategoryName = "Headphones & Audio", Rating = 4 },
                new Product { Id = "sg-20", Name = "Samsung Galaxy 20", Price = 800, Currency = "CAD", Description = "TODO", CategoryId = "phones", CategoryName = "Headphones & Audio", Rating = 4 }
            };

            products.ForEach(p => p.Description = $"{lorem1} {lorem2}");
        }

        public List<Product> GetAllProducts()
        {
            var url = $"{cfg["Services:Catalog"]}/products/all";
            logger.LogInformation($"[CatalogSvc] Querying product list from: ${url}");

            var resp = httpClient.GetAsync(url).GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<List<Product>>(
                resp.Content.ReadAsStringAsync().GetAwaiter().GetResult());

        }

        public List<Category> GetCategories()
        {
            var cats = new List<Category>
            {
                new Category { Id = "sports", Name = "Sports" } ,
                new Category { Id = "games", Name = "Games" } ,
                new Category { Id = "books", Name = "Books" } ,
                new Category { Id = "computers", Name = "Computers" } ,
                new Category { Id = "tvs", Name = "TVs" } ,
                new Category { Id = "home", Name = "Home" } ,
                new Category { Id = "musical-instruments", Name = "Musical Instruments" } ,
                new Category { Id = "headphones-audio", Name = "Headphones & Audio" } ,
                new Category { Id = "phones", Name = "Phones" } ,
            };

            cats.ForEach(c => c.Description = $"Our selection for {c.Name} if amazing! {lorem2}");

            return cats;
        }

        public List<Product> GetProducts(string catId)
        {
            return products.Where(p => p.CategoryId == catId).ToList();
        }

        public Product GetProduct(string pId)
        {
            return products.FirstOrDefault(p => p.Id == pId);
        }

        public Category GetCategory(string catId)
        {
            // todo :: correctly implement with Mongo
            return GetCategories().FirstOrDefault(c => c.Id == catId);
        }
    }
}
