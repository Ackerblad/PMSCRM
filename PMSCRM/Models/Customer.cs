using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PMSCRM.Models;

public partial class Customer
{
    public Guid CustomerId { get; set; }
    [Required]
    public Guid CompanyId { get; set; }

    [StringLength(100)]
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string PhoneNumber { get; set; } = null!;

    [EmailAddress]
    [Required]
    public string EmailAddress { get; set; } = null!;

    [StringLength(100)]
    [Required]
    public string StreetAddress { get; set; } = null!;

    [StringLength(100)]
    [Required]
    public string City { get; set; } = null!;
    [StringLength(100)]
    [Required]
    public string StateOrProvince { get; set; } = null!;

    [StringLength(20)]
    [Required]
    public string PostalCode { get; set; } = null!;

    [StringLength(100)]
    [Required]
    public string Country { get; set; } = null!;
    [Required]
    public DateTime Timestamp { get; set; }

    public virtual ICollection<CommunicationLog> CommunicationLogs { get; set; } = new List<CommunicationLog>();

    public virtual Company? Company { get; set; } = null!;

    public virtual ICollection<Email> Emails { get; set; } = new List<Email>();

    public virtual ICollection<PhoneCall> PhoneCalls { get; set; } = new List<PhoneCall>();

    public virtual ICollection<TaskProcessAreaUserCustomer> TaskProcessAreaUserCustomers { get; set; } = new List<TaskProcessAreaUserCustomer>();
}
