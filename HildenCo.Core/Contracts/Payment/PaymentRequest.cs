using HildenCo.Core.Contracts.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace HildenCo.Core.Contracts.Payment
{
    public class PaymentRequest : CommandBase
    {
        public int AccountId { get; set; }
        
        public int OrderId { get; set; }

        public string Currency { get; set; }

        public string Number { get; set; }

        public decimal Amount { get; set; }

        /// <summary>
        /// Payment Method: MasterCard, Visa, Amex, etc
        /// </summary>
        public string Method { get; set; }

        public string Name { get; set; }

        public DateTime ExpDate { get; set; }

        public int CVV { get; set; }

        /// <summary>
        /// A fake result just to simulate approved and declined payments 
        /// </summary>
        public bool FakeResult { get; set; }

        /// <summary>
        /// A Fake delay in ms to simulate delays to the payment gateway
        /// </summary>
        public int FakeDelay { get; set; } = 5000;
    }
}
