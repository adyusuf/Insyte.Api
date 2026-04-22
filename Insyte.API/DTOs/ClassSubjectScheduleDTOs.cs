using Insyte.Core.Enums;

namespace Insyte.API.DTOs;

// Class DTOs
public record ClassDto(
    Guid Id,
    Guid SchoolId,
    string Name,
    ClassLevel Level,
    string Type,
    string? Description,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record CreateClassRequest(
    string Name,
    ClassLevel Level,
    string Type,
    string? Description
);

public record UpdateClassRequest(
    string? Name,
    ClassLevel? Level,
    string? Type,
    string? Description,
    bool? IsActive
);

// Subject DTOs
public record SubjectDto(
    Guid Id,
    Guid SchoolId,
    string Name,
    string Branch,
    string Level,
    string? Description,
    int WeeklyHours,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record CreateSubjectRequest(
    string Name,
    string Branch,
    string Level,
    string? Description,
    int WeeklyHours
);

public record UpdateSubjectRequest(
    string? Name,
    string? Branch,
    string? Level,
    string? Description,
    int? WeeklyHours,
    bool? IsActive
);

// Schedule DTOs
public record ScheduleDto(
    Guid Id,
    Guid SchoolId,
    Guid ClassId,
    Guid SubjectId,
    Guid TeacherUserId,
    ScheduleDay DayOfWeek,
    TimeSpan StartTime,
    TimeSpan EndTime,
    string? Room,
    string? Notes,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record CreateScheduleRequest(
    Guid ClassId,
    Guid SubjectId,
    Guid TeacherUserId,
    ScheduleDay DayOfWeek,
    TimeSpan StartTime,
    TimeSpan EndTime,
    string? Room,
    string? Notes
);

public record UpdateScheduleRequest(
    Guid? ClassId,
    Guid? SubjectId,
    Guid? TeacherUserId,
    ScheduleDay? DayOfWeek,
    TimeSpan? StartTime,
    TimeSpan? EndTime,
    string? Room,
    string? Notes,
    bool? IsActive
);

// Schedule Conflict DTO
public record ScheduleConflict(
    Guid Schedule1Id,
    Guid Schedule2Id,
    string ConflictDescription
);

public record CheckScheduleConflictRequest(
    Guid ClassId,
    ScheduleDay DayOfWeek,
    TimeSpan StartTime,
    TimeSpan EndTime
);

public record ScheduleConflictResponse(
    bool HasConflict,
    List<ScheduleConflict> Conflicts
);
