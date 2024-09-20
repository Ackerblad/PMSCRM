using System;
using System.Collections.Generic;

namespace PMSCRM.Models;

public partial class Task
{
    public Guid TaskId { get; set; }

    public Guid CompanyId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public byte Duration { get; set; }

    public DateTime? Timestamp { get; set; }

    public virtual ICollection<CommunicationLog> CommunicationLogs { get; set; } = new List<CommunicationLog>();

    // Ändrade Company till Nullable för att få postman att funka.
    public virtual Company? Company { get; set; }

    public virtual ICollection<TaskProcessArea> TaskProcessAreas { get; set; } = new List<TaskProcessArea>();

    public Task()
    {
        
    }

    public Task(Guid taskId, Guid companyId, string name, string? description, byte duration, DateTime? timestamp, ICollection<CommunicationLog> communicationLogs, ICollection<TaskProcessArea> taskProcessAreas)
    {
        TaskId = taskId;
        CompanyId = companyId;
        Name = name;
        Description = description;
        Duration = duration;
        Timestamp = timestamp;
        CommunicationLogs = communicationLogs;
        TaskProcessAreas = taskProcessAreas;
    }
}
