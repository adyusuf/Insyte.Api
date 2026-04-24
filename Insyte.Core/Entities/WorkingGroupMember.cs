namespace Insyte.Core.Entities;

public class WorkingGroupMember
{
    public Guid Id { get; set; }
    public Guid WorkingGroupId { get; set; }
    public Guid UserId { get; set; }
    public string? Role { get; set; } // Başkan, Üye, vb.
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public WorkingGroup? WorkingGroup { get; set; }
    public User? User { get; set; }
}
