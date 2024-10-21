using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PMSCRM.Models;

public partial class Process
{
    public Guid ProcessId { get; set; }

    public Guid CompanyId { get; set; }

    public Guid AreaId { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(255)]
    public string? Description { get; set; }

    [Required]
    [Range(0, 255)]
    public byte Duration { get; set; }

    public DateTime Timestamp { get; set; }

    public virtual Area? Area { get; set; } = null!;

    public virtual Company? Company { get; set; } = null!;

    public virtual ICollection<TaskProcessArea> TaskProcessAreas { get; set; } = new List<TaskProcessArea>();
}
