using System;
using HildenCo.Core.Infrastructure.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Web.Models.Account
{
    public class Address
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public bool IsDefault { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Street { get; set; }

        [Required]
        [StringLength(10)]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(2)]
        public string Region { get; set; }

        [Required]
        [StringLength(20)]
        public string Country { get; set; }

        // simple validation
        public bool IsValid() =>
            Name.HasValue(3) &&
            PostalCode.HasValue(3) &&
            City.HasValue(3) &&
            Region.HasValue(2) &&
            Country.HasValue(2);
    }
}
