using System;
using System.Collections.Generic;

namespace PMSCRM.Models;
public partial class TaskProcessArea
{
    public Guid TaskProcessAreaId { get; set; }

    public Guid CompanyId { get; set; }

    public Guid TaskId { get; set; }

    public Guid ProcessId { get; set; }

    public Guid AreaId { get; set; }

    public DateTime? Timestamp { get; set; }

    public virtual Area Area { get; set; } = null!;

    public virtual Company Company { get; set; } = null!;

    public virtual Process Process { get; set; } = null!;

    public virtual Task Task { get; set; } = null!;

    public virtual ICollection<TaskProcessAreaUserCustomer> TaskProcessAreaUserCustomers { get; set; } = new List<TaskProcessAreaUserCustomer>();

    public TaskProcessArea()
    {
    }
}
