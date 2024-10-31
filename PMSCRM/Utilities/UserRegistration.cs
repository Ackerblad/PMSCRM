﻿
using System.ComponentModel.DataAnnotations;

namespace PMSCRM.Utilities
{
    public class UserRegistration
    {
        [Required]
        public Guid RoleId { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string EmailAddress { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string PhoneNumber { get; set; } = null!;
    }
}
