using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Models
{
    public class LineItem
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public float Price { get; set; }
        public int Qty { get; set; }
    }
}
