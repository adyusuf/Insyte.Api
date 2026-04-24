namespace Insyte.Core.Entities;

public class Council
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid SchoolId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public School? School { get; set; }
    public ICollection<CouncilMember> Members { get; set; } = new List<CouncilMember>();
}
