using Insyte.Core.Enums;

namespace Insyte.Core.Entities;

public class Report
{
    public Guid Id { get; set; }
    public Guid EvaluationId { get; set; }
    public string? PdfPath { get; set; }
    public Guid? ApprovedByUserId { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public ReportStatus Status { get; set; } = ReportStatus.Draft;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Evaluation Evaluation { get; set; } = null!;
    public User? ApprovedBy { get; set; }
    public ICollection<EmailLog> EmailLogs { get; set; } = new List<EmailLog>();
}
