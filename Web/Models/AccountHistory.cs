using System;
using System.ComponentModel.DataAnnotations;
using Core.Infrastructure.Extentions;

namespace Web.Models
{
    public class AccountHistory
    {
        public string Info { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
