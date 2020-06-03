using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentSvc.Models
{
    public class PaymentGatewayResponse
    {
        public string PaymentGatewayId { get; set; }
        
        public string Currency { get; set; }

        public decimal Amount { get; set; }

        public string AuthCode { get; set; }

        public PaymentGatewayResponseStatus Status { get; set; }

        public DateTime RequestedOn { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Just another fake val in the future
        /// </summary>
        public DateTime ProcessedOn { get; set; } = DateTime.UtcNow.AddSeconds(13);
    }
}
