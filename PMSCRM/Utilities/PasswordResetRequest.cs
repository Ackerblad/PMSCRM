using System.ComponentModel.DataAnnotations;

namespace PMSCRM.Utilities
{
    public class PasswordResetRequest
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } = null!;
    }
}
