using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class CartReview
    {
        public Account Account { get; set; }
        public Address Address { get; set; }
        public PaymentInfo PaymentInfo { get; set; }
    }
}
