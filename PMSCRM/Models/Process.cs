using System;
using System.Collections.Generic;

namespace PMSCRM.Models;

public partial class Process
{
    public Guid ProcessId { get; set; }

    public Guid CompanyId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public byte Duration { get; set; }

    public DateTime Timestamp { get; set; }

    public virtual Company? Company { get; set; } = null!;

    public virtual ICollection<TaskProcessArea> TaskProcessAreas { get; set; } = new List<TaskProcessArea>();
}
