using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentSvc.Models
{
    public class PaymentGatewayRequest
    {
        public int Id { get; set; }
        
        public int PaymentId { get; set; }

        /// <summary>
        /// A random value to emulate a payment gateway Id
        /// </summary>
        public string PaymentGatewayId { get; set; } = Guid.NewGuid().ToString();
        
        public string Currency { get; set; }

        public decimal Amount { get; set; }

        public PaymentMethod Method { get; set; }

        public string Name { get; set; }

        public string Number { get; set; }

        public DateTime ExpDate { get; set; }

        public int CVV { get; set; }

        public string AuthCode { get; set; }

        public PaymentGatewayResponseStatus Status { get; set; }

        public PaymentStatus FakeStatus { get; set; }

        public int FakeDelay { get; set; } = 5000;
    }
}
