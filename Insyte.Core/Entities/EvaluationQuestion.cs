namespace Insyte.Core.Entities;

public class EvaluationQuestion
{
    public Guid Id { get; set; }
    public Guid CriteriaId { get; set; }
    public string Question { get; set; } = null!;
    public string? Category { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public EvaluationCriteria Criteria { get; set; } = null!;
}
