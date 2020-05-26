using HildenCo.Core.Contracts.Shipping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OrderSvc.Models
{
    public class ShippingInfo
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public ShippingStatus Status { get; set; }

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

        /// <summary>
        /// Shipping amount (not implemented)
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// ShippingProvider (not implemented)
        /// </summary>
        public ShippingProvider Provider { get; set; }


        /// <summary>
        /// Basic override for logging purposes
        /// </summary>
        /// <returns></returns>
        public override string ToString() =>
            $"Name: {Name}, Address: {Street}, {City} - {Region}, {Country}";
    }
}
