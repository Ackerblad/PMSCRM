

namespace PMSCRM.Models;

public partial class User
{
    public Guid UserId { get; set; }

    public Guid CompanyId { get; set; }

    public Guid RoleId { get; set; }

    public string EmailAddress { get; set; } = null!;

    public string Password { get; set; } = null!;

    public Guid ResetToken { get; set; }

    public DateTime ResetTokenExpiryDate { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    public virtual Company? Company { get; set; } = null!;

    public virtual ICollection<Email> Emails { get; set; } = [];

    public virtual ICollection<PhoneCall> PhoneCalls { get; set; } = [];

    public virtual Role? Role { get; set; } = null!;

    public virtual ICollection<TaskProcessAreaUserCustomer> TaskProcessAreaUserCustomers { get; set; } = [];
}
