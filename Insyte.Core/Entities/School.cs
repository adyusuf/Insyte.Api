using Insyte.Core.Enums;

namespace Insyte.Core.Entities;

public class School
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string? LogoPath { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public SchoolType SchoolType { get; set; }
    public InstitutionType InstitutionType { get; set; }
    public LiseType? LiseType { get; set; }
    public EducationSystem? EducationSystem { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public ICollection<SchoolAdvisor> Advisors { get; set; } = new List<SchoolAdvisor>();
    public ICollection<SchoolUser> Users { get; set; } = new List<SchoolUser>();
    public ICollection<Video> Videos { get; set; } = new List<Video>();
    public ICollection<EmailConfig> EmailConfigs { get; set; } = new List<EmailConfig>();
    public ICollection<Class> Classes { get; set; } = new List<Class>();
    public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
    public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    public ICollection<SchoolFacility> Facilities { get; set; } = new List<SchoolFacility>();
    public ICollection<SchoolServiceOffering> Services { get; set; } = new List<SchoolServiceOffering>();
    public ICollection<SchoolActivity> Activities { get; set; } = new List<SchoolActivity>();
    public ICollection<SchoolLanguage> Languages { get; set; } = new List<SchoolLanguage>();
}
