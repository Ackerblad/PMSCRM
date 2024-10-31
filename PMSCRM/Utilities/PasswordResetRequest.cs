using System.ComponentModel.DataAnnotations;

namespace PMSCRM.Utilities
{
    public class PasswordResetRequest
    {
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string EmailAddress { get; set; } = null!;
    }
}
