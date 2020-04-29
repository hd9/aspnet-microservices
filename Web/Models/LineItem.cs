using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Web.Models
{
    public class LineItem
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        [Range(0,100000)]
        [Required]
        public decimal Price { get; set; }

        [Range(1, 999)]
        [Required]
        public int Qty { get; set; } = 1;
    }
}
