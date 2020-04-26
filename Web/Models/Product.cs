using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public string Currency { get; set; }
        public float OrderTotal { get; set; }
        public float Taxes { get; set; }
        public float Total { get; set; }
        public string Url { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int Rating { get; set; }
    }
}
