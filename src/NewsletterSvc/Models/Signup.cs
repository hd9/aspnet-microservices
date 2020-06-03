using System;
using System.Collections.Generic;
using System.Text;

namespace NewsletterSvc.Models
{
    public class Signup
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
