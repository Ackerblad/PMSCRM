using System;
using System.Collections.Generic;

namespace PMSCRM.Models;

public partial class CommunicationLog
{
    public Guid CommunicationLogId { get; set; }

    public Guid CompanyId { get; set; }

    public Guid CustomerId { get; set; }

    public Guid TaskId { get; set; }

    public Guid? EmailId { get; set; }

    public Guid? PhoneCallId { get; set; }

    public DateTime? LogDate { get; set; }

    public string? Notes { get; set; }

    public DateTime? Timestamp { get; set; }

    public virtual Company? Company { get; set; } = null!;

    public virtual Customer? Customer { get; set; } = null!;

    public virtual Email? Email { get; set; }

    public virtual PhoneCall? PhoneCall { get; set; }

    public virtual Task? Task { get; set; } = null!;

    public CommunicationLog()
    {
    }
}
