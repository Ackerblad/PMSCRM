using System.ComponentModel.DataAnnotations;

namespace PMSCRM.Utilities
{
    public class PasswordReset
    {
        [Required]
        public Guid Token { get; set; }

        [Required]
        public string NewPassword { get; set; } = null!;
    }
}
