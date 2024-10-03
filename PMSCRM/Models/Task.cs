using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PMSCRM.Models;

public partial class Task
{
    public Guid TaskId { get; set; }
    [Required]
    public Guid CompanyId { get; set; }
    [Required]
    [StringLength(50, ErrorMessage = "The name must be less than 50 characters.")]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }
    [Required]
    public byte Duration { get; set; }

    public DateTime Timestamp { get; set; }

    public virtual ICollection<CommunicationLog> CommunicationLogs { get; set; } = new List<CommunicationLog>();

    public virtual Company? Company { get; set; } = null!;

    public virtual ICollection<TaskProcessArea> TaskProcessAreas { get; set; } = new List<TaskProcessArea>();
}
