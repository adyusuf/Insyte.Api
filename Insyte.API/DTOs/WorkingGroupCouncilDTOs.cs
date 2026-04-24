namespace Insyte.API.DTOs;

// Working Groups
public record WorkingGroupDto(
    Guid Id,
    string Name,
    string? Description,
    Guid SchoolId,
    bool IsActive,
    int MemberCount,
    DateTime CreatedAt
);

public record CreateWorkingGroupRequest(
    string Name,
    string? Description
);

public record UpdateWorkingGroupRequest(
    string? Name,
    string? Description,
    bool? IsActive
);

public record WorkingGroupMemberDto(
    Guid Id,
    Guid WorkingGroupId,
    Guid UserId,
    string FirstName,
    string LastName,
    string Email,
    string? Role,
    DateTime AssignedAt
);

public record AddWorkingGroupMemberRequest(
    Guid UserId,
    string Role = "Member"
);

// Councils
public record CouncilDto(
    Guid Id,
    string Name,
    string? Description,
    Guid SchoolId,
    bool IsActive,
    int MemberCount,
    DateTime CreatedAt
);

public record CreateCouncilRequest(
    string Name,
    string? Description
);

public record UpdateCouncilRequest(
    string? Name,
    string? Description,
    bool? IsActive
);

public record CouncilMemberDto(
    Guid Id,
    Guid CouncilId,
    Guid UserId,
    string FirstName,
    string LastName,
    string Email,
    string? Role,
    DateTime AssignedAt
);

public record AddCouncilMemberRequest(
    Guid UserId,
    string Role = "Member"
);
