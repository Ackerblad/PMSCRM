

namespace PMSCRM.Models;

public partial class Email
{
    public Guid EmailId { get; set; }

    public Guid CompanyId { get; set; }

    public Guid CustomerId { get; set; }

    public Guid UserId { get; set; }

    public string Sender { get; set; } = null!;

    public string Recipient { get; set; } = null!;

    public string? Subject { get; set; }

    public string? Body { get; set; }

    public DateTime SentDate { get; set; }

    public bool Received { get; set; }

    public string? ApiResponse { get; set; }

    public byte Status { get; set; }

    public DateTime Timestamp { get; set; }

    public virtual ICollection<CommunicationLog> CommunicationLogs { get; set; } = [];

    public virtual Company? Company { get; set; } = null!;

    public virtual Customer? Customer { get; set; } = null!;

    public virtual User? User { get; set; } = null!;
}
