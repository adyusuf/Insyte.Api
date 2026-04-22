namespace Insyte.API.DTOs;

public record AIProviderDto(
    Guid Id,
    string Name,
    string Provider,
    string? BaseUrl,
    bool IsActive,
    DateTime CreatedAt,
    int ModelCount
);

public record CreateAIProviderRequest(
    string Name,
    string Provider,
    string? ApiKey,
    string? BaseUrl
);

public record UpdateAIProviderRequest(
    string? Name,
    string? Provider,
    string? ApiKey,
    string? BaseUrl,
    bool? IsActive
);

public record AIModelDto(
    Guid Id,
    Guid AIProviderId,
    string ProviderName,
    string Name,
    string ModelId,
    int MaxTokens,
    bool IsActive,
    DateTime CreatedAt
);

public record CreateAIModelRequest(
    string Name,
    string ModelId,
    int MaxTokens = 4096
);

public record UpdateAIModelRequest(
    string? Name,
    string? ModelId,
    int? MaxTokens,
    bool? IsActive
);

public record CriteriaDto(
    Guid Id,
    string Name,
    string? Description,
    string Instructions,
    string? Subject,
    bool IsActive,
    DateTime CreatedAt
);

public record CreateCriteriaRequest(
    string Name,
    string? Description,
    string Instructions,
    string? Subject
);

public record UpdateCriteriaRequest(
    string? Name,
    string? Description,
    string? Instructions,
    string? Subject,
    bool? IsActive
);

public record CriteriaWithQuestionsDto(
    Guid Id,
    string Name,
    string? Description,
    string Instructions,
    string? Subject,
    bool IsActive,
    DateTime CreatedAt,
    List<EvaluationQuestionDto> Questions
);
