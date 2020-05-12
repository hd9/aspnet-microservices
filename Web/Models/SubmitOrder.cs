using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class SubmitOrder : Order
    {
        public string AddressId { get; set; }
        public string PaymentId { get; set; }
    }
}
