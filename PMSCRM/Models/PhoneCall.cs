using System;
using System.Collections.Generic;

namespace PMSCRM.Models;

public partial class PhoneCall
{
    public Guid PhoneCallId { get; set; }

    public Guid CompanyId { get; set; }

    public Guid CustomerId { get; set; }

    public Guid UserId { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public byte? Duration { get; set; }

    public byte CallType { get; set; }

    public string? ApiResponse { get; set; }

    public byte Status { get; set; }

    public DateTime Timestamp { get; set; }

    public virtual ICollection<CommunicationLog> CommunicationLogs { get; set; } = new List<CommunicationLog>();

    public virtual Company? Company { get; set; } = null!;

    public virtual Customer? Customer { get; set; } = null!;

    public virtual User? User { get; set; } = null!;
}
