namespace Insyte.Core.Entities;

public class AIModel
{
    public Guid Id { get; set; }
    public Guid AIProviderId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ModelId { get; set; } = string.Empty; // e.g. gpt-4o, claude-sonnet-4-20250514
    public int MaxTokens { get; set; } = 4096;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public AIProvider AIProvider { get; set; } = null!;
    public ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();
}
