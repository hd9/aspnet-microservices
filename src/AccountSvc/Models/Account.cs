using System;
using System.Collections.Generic;
using System.Text;

namespace AccountSvc.Models
{
    public class Account
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public bool SubscribedToNewsletter { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}
