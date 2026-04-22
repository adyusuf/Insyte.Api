using Insyte.Core.Enums;

namespace Insyte.Core.Entities;

public class Video
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string? OriginalFileName { get; set; }
    public long FileSize { get; set; }
    public Guid SchoolId { get; set; }
    public Guid TeacherUserId { get; set; }
    public Guid UploadedByUserId { get; set; }
    public string? Subject { get; set; }
    public VideoStatus Status { get; set; } = VideoStatus.Uploaded;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public School School { get; set; } = null!;
    public User Teacher { get; set; } = null!;
    public User UploadedBy { get; set; } = null!;
    public ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();
}
