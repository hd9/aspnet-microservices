﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Models.Catalog
{
    public class Category
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
    }
}
