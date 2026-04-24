namespace Insyte.Core.Entities;

public class CouncilMember
{
    public Guid Id { get; set; }
    public Guid CouncilId { get; set; }
    public Guid UserId { get; set; }
    public string? Role { get; set; } // Başkan, Başkan Yardımcısı, Üye, vb.
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Council? Council { get; set; }
    public User? User { get; set; }
}
