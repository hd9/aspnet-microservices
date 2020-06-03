using System;
using Microservices.Core.Contracts.Payment;

namespace PaymentSvc.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public int AccountId { get; set; }
        
        public int OrderId { get; set; }
        
        public decimal Amount { get; set; }
        
        public string Currency { get; set; }
        
        public PaymentStatus Status { get; set; }

        public PaymentMethod Method { get; set; }

        public string AuthCode { get; set; }

        public static Payment Parse(PaymentRequest pr)
        {
            if (pr == null)
                return null;

            // todo :: automapper
            return new Payment
            {
                AccountId = pr.AccountId,
                Currency = pr.Currency,
                OrderId = pr.OrderId,
                Amount = pr.Amount,
                Method = Enum.Parse<PaymentMethod>(pr.Method, true)
            };
        }
    }
}
