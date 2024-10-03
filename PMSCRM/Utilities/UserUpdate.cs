namespace PMSCRM.Utilities
{
    public class UserUpdate
    {
        public string EmailAddress { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public Guid CompanyId { get; set; }
        public Guid RoleId { get; set; }
    }
}
