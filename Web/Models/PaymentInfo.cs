using System;
using System.ComponentModel.DataAnnotations;
using Core.Infrastructure.Extensions;

namespace Web.Models
{
    public enum PaymentMethod
    {
        MasterCard,
        Visa,
        Amex
    }

    public class PaymentInfo
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public bool IsDefault { get; set; }
        
        [Required]
        public PaymentMethod Method { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.CreditCard)]
        [StringLength(20)]
        public string Number { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ExpDate { get; set; }

        [Required]
        [Range(1, 999)]
        public int CVV { get; set; }

        // simple validation
        public bool IsValid() =>
            Id > 0 &&
            AccountId > 0 &&
            Name.HasValue(3) &&
            Number.HasValue(10) &&
            (CVV > 0 && CVV < 999) &&
            ExpDate > DateTime.UtcNow.AddDays(1);
    }
}
