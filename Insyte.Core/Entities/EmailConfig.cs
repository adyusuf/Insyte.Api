using Insyte.Core.Enums;

namespace Insyte.Core.Entities;

public class EmailConfig
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public string RecipientEmail { get; set; } = string.Empty;
    public string? RecipientName { get; set; }
    public RecipientType RecipientType { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public School School { get; set; } = null!;
}
