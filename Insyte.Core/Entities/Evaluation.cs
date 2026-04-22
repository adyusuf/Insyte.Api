using Insyte.Core.Enums;

namespace Insyte.Core.Entities;

public class Evaluation
{
    public Guid Id { get; set; }
    public Guid VideoId { get; set; }
    public Guid CriteriaId { get; set; }
    public Guid AIModelId { get; set; }
    public string? Result { get; set; } // JSON result from AI
    public int TokenUsageInput { get; set; }
    public int TokenUsageOutput { get; set; }
    public EvaluationStatus Status { get; set; } = EvaluationStatus.Pending;
    public string? ErrorMessage { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }

    // Navigation
    public Video Video { get; set; } = null!;
    public EvaluationCriteria Criteria { get; set; } = null!;
    public AIModel AIModel { get; set; } = null!;
    public Report? Report { get; set; }
}
