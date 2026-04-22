using Insyte.Core.Enums;

namespace Insyte.Core.Entities;

public class SchoolFacility
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public PhysicalFacility Facility { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public School School { get; set; } = null!;
}

public class SchoolServiceOffering
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public SchoolService Service { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public School School { get; set; } = null!;
}

public class SchoolActivity
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public Activity Activity { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public School School { get; set; } = null!;
}

public class SchoolLanguage
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public ForeignLanguage Language { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public School School { get; set; } = null!;
}
