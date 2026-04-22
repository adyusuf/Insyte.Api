using Insyte.Core.Enums;

namespace Insyte.Core.Entities;

public class Schedule
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public Guid ClassId { get; set; }
    public Guid SubjectId { get; set; }
    public Guid TeacherUserId { get; set; }
    public ScheduleDay DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string? Room { get; set; } // Sınıf odası/yer bilgisi
    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public School School { get; set; } = null!;
    public Class Class { get; set; } = null!;
    public Subject Subject { get; set; } = null!;
    public User Teacher { get; set; } = null!;
}
