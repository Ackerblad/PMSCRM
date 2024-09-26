namespace PMSCRM.Utilities
{
    public class PasswordReset
    {
        public string Token { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
