namespace PMSCRM.Utilities
{
    public class PasswordReset
    {
        public Guid Token { get; set; }
        public string NewPassword { get; set; } = null!;
    }
}
