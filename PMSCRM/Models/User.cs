using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PMSCRM.Models;

public partial class User
{
    public Guid UserId { get; set; }

    public Guid CompanyId { get; set; }

    public Guid RoleId { get; set; }

    [StringLength(100)]
    [Required]
    public string EmailAddress { get; set; } = null!;

    [StringLength(64)]
    public string Password { get; set; } = null!;

    public Guid ResetToken { get; set; }

    public DateTime ResetTokenExpiryDate { get; set; }

    [StringLength(100)]
    [Required]
    public string FirstName { get; set; } = null!;

    [StringLength(100)]
    [Required]
    public string LastName { get; set; } = null!;

    [StringLength(100)]
    [Required]
    public string PhoneNumber { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    public virtual Company? Company { get; set; } = null!;

    public virtual ICollection<Email> Emails { get; set; } = new List<Email>();

    public virtual ICollection<PhoneCall> PhoneCalls { get; set; } = new List<PhoneCall>();

    public virtual Role? Role { get; set; } = null!;

    public virtual ICollection<TaskProcessAreaUserCustomer> TaskProcessAreaUserCustomers { get; set; } = new List<TaskProcessAreaUserCustomer>();
}
