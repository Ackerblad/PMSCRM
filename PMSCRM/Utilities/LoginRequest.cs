using System.ComponentModel.DataAnnotations;

namespace PMSCRM.Utilities
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
