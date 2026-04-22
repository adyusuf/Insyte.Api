namespace Insyte.API.DTOs;

public record EvaluationQuestionDto(
    Guid Id,
    Guid CriteriaId,
    string Question,
    string? Category,
    int Order,
    bool IsActive,
    DateTime CreatedAt
);

public record CreateEvaluationQuestionRequest(
    string Question,
    string? Category,
    int Order
);

public record UpdateEvaluationQuestionRequest(
    string? Question,
    string? Category,
    int? Order,
    bool? IsActive
);
