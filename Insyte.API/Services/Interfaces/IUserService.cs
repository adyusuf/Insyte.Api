using Insyte.API.DTOs;

namespace Insyte.API.Services.Interfaces;

public interface IUserService
{
    Task<PagedResult<UserDto>> GetAllAsync(string? search, int page, int pageSize);
    Task<UserDto?> GetByIdAsync(Guid id);
    Task<(bool Success, string? Error, UserDto? User)> CreateAsync(CreateUserRequest request);
    Task<(bool Success, string? Error, UserDto? User)> UpdateAsync(Guid id, UpdateUserRequest request);
    Task<(bool Success, string? Error)> DeleteAsync(Guid id, Guid currentUserId);
}
