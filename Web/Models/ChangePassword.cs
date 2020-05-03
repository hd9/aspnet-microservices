﻿using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class ChangePassword
    {
        public string AccountId { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }

    }
}
