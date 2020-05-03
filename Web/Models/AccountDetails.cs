using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class AccountDetails
    {
        public string AccountId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
