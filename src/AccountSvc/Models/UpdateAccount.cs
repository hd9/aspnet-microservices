using System.ComponentModel.DataAnnotations;

namespace AccountSvc.Models
{
    public class UpdateAccount
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
