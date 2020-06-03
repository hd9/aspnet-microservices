using System.ComponentModel.DataAnnotations;

namespace Web.Models.Account
{
    public class AccountDetails
    {
        public string Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
