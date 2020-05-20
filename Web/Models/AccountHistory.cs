using System;
using System.ComponentModel.DataAnnotations;
using Core.Infrastructure.Extensions;

namespace Web.Models
{
    public class AccountHistory
    {
        public string Info { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
