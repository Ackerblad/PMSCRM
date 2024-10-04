namespace PMSCRM.Utilities
{
    public class LoginRequest
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
