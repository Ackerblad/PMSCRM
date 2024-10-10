using PMSCRM.Models;
using System.ComponentModel.DataAnnotations;

namespace PMSCRM.Utilities
{
    public class UserRegistration
    {
        [Required]
        public Guid RoleId { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } = null!;

        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
