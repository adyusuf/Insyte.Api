namespace Insyte.Core.Entities;

public class EvaluationCriteria
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Instructions { get; set; } = string.Empty; // JSON - AI instructions
    public string? Subject { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();
    public ICollection<EvaluationQuestion> Questions { get; set; } = new List<EvaluationQuestion>();
}
