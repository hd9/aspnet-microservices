using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Models
{
    public class Order
    {
        public string AccountId { get; set; }
        public List<LineItem> LineItems { get; set; }
    }
}
