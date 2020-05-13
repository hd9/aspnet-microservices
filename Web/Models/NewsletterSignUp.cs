using Core.Infrastructure.Extentions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Models
{
    public class NewsletterSignUp
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public bool IsValid() =>
            Name.HasValue(3) &&
            Email.HasValue(6);
    }
}
