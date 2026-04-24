using Insyte.Core.Enums;

namespace Insyte.Core.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public ICollection<SchoolAdvisor> AdvisedSchools { get; set; } = new List<SchoolAdvisor>();
    public ICollection<SchoolUser> SchoolUsers { get; set; } = new List<SchoolUser>();
    public ICollection<Video> UploadedVideos { get; set; } = new List<Video>();
    public ICollection<WorkingGroupMember> WorkingGroupMemberships { get; set; } = new List<WorkingGroupMember>();
    public ICollection<CouncilMember> CouncilMemberships { get; set; } = new List<CouncilMember>();
}
