using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PMSCRM.Models;

public partial class Customer
{
    public Guid CustomerId { get; set; }

    public Guid CompanyId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string EmailAddress { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string StreetAddress { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string City { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string StateOrProvince { get; set; } = null!;

    [Required]
    [StringLength(20)]
    public string PostalCode { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Country { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    public virtual ICollection<CommunicationLog> CommunicationLogs { get; set; } = new List<CommunicationLog>();

    public virtual Company? Company { get; set; } = null!;

    public virtual ICollection<Email> Emails { get; set; } = new List<Email>();

    public virtual ICollection<PhoneCall> PhoneCalls { get; set; } = new List<PhoneCall>();

    public virtual ICollection<TaskProcessAreaUserCustomer> TaskProcessAreaUserCustomers { get; set; } = new List<TaskProcessAreaUserCustomer>();
}
