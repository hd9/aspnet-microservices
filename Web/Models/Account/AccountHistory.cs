using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.Account
{
    public class AccountHistory
    {
        public string Info { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
