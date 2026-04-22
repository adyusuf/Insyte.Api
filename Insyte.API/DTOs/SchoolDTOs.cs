using Insyte.Core.Enums;

namespace Insyte.API.DTOs;

public record SchoolDto(
    Guid Id,
    string Name,
    string? Address,
    string? City,
    string? Phone,
    string? Email,
    bool IsActive,
    DateTime CreatedAt,
    int AdvisorCount,
    int TeacherCount,
    int VideoCount
);

public record CreateSchoolRequest(
    string Name,
    string? Address,
    string? City,
    string? Phone,
    string? Email
);

public record UpdateSchoolRequest(
    string? Name,
    string? Address,
    string? City,
    string? Phone,
    string? Email,
    bool? IsActive
);

public record AssignAdvisorRequest(Guid UserId);

public record SchoolTeacherDto(
    Guid Id,
    Guid UserId,
    string FirstName,
    string LastName,
    string Email,
    UserRole Role,
    DateTime AssignedAt
);

public record AssignTeacherRequest(Guid UserId, UserRole Role);

public record EmailConfigDto(
    Guid Id,
    Guid SchoolId,
    string RecipientEmail,
    string? RecipientName,
    RecipientType RecipientType,
    bool IsActive
);

public record CreateEmailConfigRequest(
    string RecipientEmail,
    string? RecipientName,
    RecipientType RecipientType
);
