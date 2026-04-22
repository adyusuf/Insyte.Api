using Insyte.Core.Enums;

namespace Insyte.API.DTOs;

public record VideoDto(
    Guid Id,
    string Title,
    string? OriginalFileName,
    long FileSize,
    Guid SchoolId,
    string SchoolName,
    Guid TeacherUserId,
    string TeacherName,
    string? Subject,
    VideoStatus Status,
    DateTime CreatedAt,
    int EvaluationCount
);

public record CreateVideoRequest(
    string Title,
    Guid SchoolId,
    Guid TeacherUserId,
    string? Subject
);

public record EvaluateVideoRequest(
    Guid CriteriaId,
    Guid AIModelId,
    string? CustomInstructions
);
