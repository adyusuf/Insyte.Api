using Insyte.Core.Enums;

namespace Insyte.API.DTOs;

public record EvaluationDto(
    Guid Id,
    Guid VideoId,
    string VideoTitle,
    Guid CriteriaId,
    string CriteriaName,
    Guid AIModelId,
    string AIModelName,
    string? Result,
    int TokenUsageInput,
    int TokenUsageOutput,
    EvaluationStatus Status,
    string? ErrorMessage,
    DateTime CreatedAt,
    DateTime? CompletedAt
);

public record ReportDto(
    Guid Id,
    Guid EvaluationId,
    string VideoTitle,
    string SchoolName,
    string TeacherName,
    string? PdfPath,
    string? ApprovedByName,
    DateTime? ApprovedAt,
    ReportStatus Status,
    DateTime CreatedAt
);
