using System;
using System.Collections.Generic;

namespace PMSCRM.Models;
public partial class Customer
{
    public Guid CustomerId { get; set; }

    public Guid CompanyId { get; set; }

    public string Name { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;

    public string StreetAddress { get; set; } = null!;

    public string City { get; set; } = null!;

    public string? StateOrProvince { get; set; }

    public string? PostalCode { get; set; }

    public string Country { get; set; } = null!;

    public DateTime? Timestamp { get; set; }

    public virtual ICollection<CommunicationLog> CommunicationLogs { get; set; } = new List<CommunicationLog>();

    public virtual Company? Company { get; set; } = null!;

    public virtual ICollection<Email> Emails { get; set; } = new List<Email>();

    public virtual ICollection<PhoneCall> PhoneCalls { get; set; } = new List<PhoneCall>();

    public virtual ICollection<TaskProcessAreaUserCustomer> TaskProcessAreaUserCustomers { get; set; } = new List<TaskProcessAreaUserCustomer>();

    public Customer()
    {
    }
}
