using Insyte.Core.Enums;

namespace Insyte.Core.Entities;

public class EmailLog
{
    public Guid Id { get; set; }
    public Guid ReportId { get; set; }
    public string RecipientEmail { get; set; } = string.Empty;
    public DateTime? SentAt { get; set; }
    public EmailStatus Status { get; set; } = EmailStatus.Pending;
    public string? ErrorMessage { get; set; }

    // Navigation
    public Report Report { get; set; } = null!;
}
