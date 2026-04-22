namespace Insyte.Core.Entities;

public class SchoolAdvisor
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public Guid UserId { get; set; }
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public School School { get; set; } = null!;
    public User User { get; set; } = null!;
}
