namespace Insyte.Core.Entities;

public class AIProvider
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty; // openai, anthropic, google
    public string? ApiKey { get; set; }
    public string? BaseUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<AIModel> Models { get; set; } = new List<AIModel>();
}
