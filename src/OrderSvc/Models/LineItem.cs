﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OrderSvc.Models
{
    public class LineItem
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Slug { get; set; }

        [Range(0,100000)]
        [Required]
        public decimal Price { get; set; }

        [Range(1, 999)]
        [Required]
        public int Qty { get; set; } = 1;
    }
}
