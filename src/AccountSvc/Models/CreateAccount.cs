using System;
using System.Collections.Generic;
using System.Text;

namespace AccountSvc.Models
{
    public class CreateAccount
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public bool SubscribedToNewsletter { get; set; }

        public string Password { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        public string Street { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public string Country { get; set; }

    }
}
