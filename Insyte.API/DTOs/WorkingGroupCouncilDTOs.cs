using System.ComponentModel.DataAnnotations;

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
    [Required(ErrorMessage = "Ad zorunludur")]
    [MinLength(2, ErrorMessage = "Ad en az 2 karakter olmalı")]
    [MaxLength(100, ErrorMessage = "Ad en fazla 100 karakter olabilir")]
    string Name,

    [MaxLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
    string? Description
);

public record UpdateWorkingGroupRequest(
    [MinLength(2, ErrorMessage = "Ad en az 2 karakter olmalı")]
    [MaxLength(100, ErrorMessage = "Ad en fazla 100 karakter olabilir")]
    string? Name,

    [MaxLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
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
    [Required(ErrorMessage = "Kullanıcı ID zorunludur")]
    Guid UserId,

    [MaxLength(50, ErrorMessage = "Rol en fazla 50 karakter olabilir")]
    string Role = "Üye"
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
    [Required(ErrorMessage = "Ad zorunludur")]
    [MinLength(2, ErrorMessage = "Ad en az 2 karakter olmalı")]
    [MaxLength(100, ErrorMessage = "Ad en fazla 100 karakter olabilir")]
    string Name,

    [MaxLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
    string? Description
);

public record UpdateCouncilRequest(
    [MinLength(2, ErrorMessage = "Ad en az 2 karakter olmalı")]
    [MaxLength(100, ErrorMessage = "Ad en fazla 100 karakter olabilir")]
    string? Name,

    [MaxLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
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
    [Required(ErrorMessage = "Kullanıcı ID zorunludur")]
    Guid UserId,

    [MaxLength(50, ErrorMessage = "Rol en fazla 50 karakter olabilir")]
    string Role = "Üye"
);
