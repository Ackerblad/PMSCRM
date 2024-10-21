using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PMSCRM.Models;

public partial class Company
{
    public Guid CompanyId { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    public virtual ICollection<Area> Areas { get; set; } = new List<Area>();

    public virtual ICollection<CommunicationLog> CommunicationLogs { get; set; } = new List<CommunicationLog>();

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Email> Emails { get; set; } = new List<Email>();

    public virtual ICollection<PhoneCall> PhoneCalls { get; set; } = new List<PhoneCall>();

    public virtual ICollection<Process> Processes { get; set; } = new List<Process>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    public virtual ICollection<TaskProcessAreaUserCustomer> TaskProcessAreaUserCustomers { get; set; } = new List<TaskProcessAreaUserCustomer>();

    public virtual ICollection<TaskProcessArea> TaskProcessAreas { get; set; } = new List<TaskProcessArea>();

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
