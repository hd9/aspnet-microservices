using Microservices.Core.Infrastructure.Extensions;
using System;
using System.ComponentModel.DataAnnotations;

namespace OrderSvc.Models
{
    public class PaymentInfo
    {
        public int Id { get; set; }

        public int AccountId { get; set; }
        
        public PaymentStatus Status { get; set; }

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

        /// <summary>
        /// Basic override for logging purposes
        /// </summary>
        /// <returns></returns>
        public override string ToString() =>
            $"Name: {Name}, Type: {Method}, Number: {Number.MaskCC()}, Exp: {ExpDate.ToExpDate()}";
    }
}
