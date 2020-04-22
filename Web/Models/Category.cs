using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Models
{
    public class Category
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url => $"/products/{(Id ?? "").ToLower()}";
    }
}
