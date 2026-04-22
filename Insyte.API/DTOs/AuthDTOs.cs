using Insyte.Core.Enums;

namespace Insyte.API.DTOs;

public record LoginRequest(string Email, string Password);
public record LoginResponse(string AccessToken, string RefreshToken, UserDto User);
public record RefreshRequest(string RefreshToken);

public record UserDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    UserRole Role,
    bool IsActive,
    DateTime CreatedAt
);

public record CreateUserRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    UserRole Role
);

public record UpdateUserRequest(
    string? Email,
    string? FirstName,
    string? LastName,
    UserRole? Role,
    bool? IsActive,
    string? Password
);
