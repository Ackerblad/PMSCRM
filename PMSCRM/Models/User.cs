using System;
using System.Collections.Generic;

namespace PMSCRM.Models;

public partial class User
{
    // Company och Role kan vara null här - jag är inte riktigt säker på hur vi löser det.
    public User()
    {
    }

    public Guid UserId { get; set; }

    public Guid CompanyId { get; set; }

    public Guid RoleId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;

    public DateTime? Timestamp { get; set; }

    public virtual Company? Company { get; set; }

    public virtual ICollection<Email> Emails { get; set; } = new List<Email>();

    public virtual ICollection<PhoneCall> PhoneCalls { get; set; } = new List<PhoneCall>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<TaskProcessAreaUserCustomer> TaskProcessAreaUserCustomers { get; set; } = new List<TaskProcessAreaUserCustomer>();

    public User(Guid userId, Guid companyId, Guid roleId, string username, string password, string firstName, string lastName, string phoneNumber, string emailAddress, DateTime? timestamp, ICollection<Email> emails, ICollection<PhoneCall> phoneCalls, ICollection<TaskProcessAreaUserCustomer> taskProcessAreaUserCustomers)
    {
        UserId = userId;
        CompanyId = companyId;
        RoleId = roleId;
        Username = username;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        EmailAddress = emailAddress;
        Timestamp = timestamp;
        Emails = emails;
        PhoneCalls = phoneCalls;
        TaskProcessAreaUserCustomers = taskProcessAreaUserCustomers;
    }
}
