using System.ComponentModel.DataAnnotations;
using Web.Models.Account;
using Acct = Web.Models.Account;

namespace Web.Models.Order
{
    public class CartReview
    {
        public Acct.Account Account { get; set; }
        public Address Address { get; set; }
        public PaymentInfo PaymentInfo { get; set; }
    }
}
