using System.ComponentModel.DataAnnotations;

namespace PMSCRM.Utilities
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(64)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
