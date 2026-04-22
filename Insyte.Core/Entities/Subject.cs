namespace Insyte.Core.Entities;

public class Subject
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty; // Branş (e.g., Matematik, Türkçe, İngilizce)
    public string Level { get; set; } = string.Empty; // Seviyesi
    public string? Description { get; set; }
    public int WeeklyHours { get; set; } = 0; // Haftada kaç saat
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public School School { get; set; } = null!;
    public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
