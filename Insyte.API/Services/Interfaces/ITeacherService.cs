using Insyte.API.DTOs;

namespace Insyte.API.Services.Interfaces;

public interface ITeacherService
{
    Task<PagedResult<UserDto>> GetAllAsync(string? search, Guid? schoolId, int page, int pageSize);
    Task<UserDto?> GetByIdAsync(Guid id);
}
