
using System.ComponentModel.DataAnnotations;

namespace PMSCRM.Models;

public partial class Company
{
    public Guid CompanyId { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    public virtual ICollection<Area> Areas { get; set; } = [];

    public virtual ICollection<CommunicationLog> CommunicationLogs { get; set; } = [];

    public virtual ICollection<Customer> Customers { get; set; } = [];

    public virtual ICollection<Email> Emails { get; set; } = [];

    public virtual ICollection<PhoneCall> PhoneCalls { get; set; } = [];

    public virtual ICollection<Process> Processes { get; set; } = [];

    public virtual ICollection<Role> Roles { get; set; } = [];

    public virtual ICollection<TaskProcessAreaUserCustomer> TaskProcessAreaUserCustomers { get; set; } = [];

    public virtual ICollection<TaskProcessArea> TaskProcessAreas { get; set; } = [];

    public virtual ICollection<Task> Tasks { get; set; } = [];

    public virtual ICollection<User> Users { get; set; } = [];
}
