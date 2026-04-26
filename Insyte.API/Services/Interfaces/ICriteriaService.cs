using Insyte.API.DTOs;

namespace Insyte.API.Services.Interfaces;

public interface ICriteriaService
{
    Task<PagedResult<CriteriaDto>> GetAllAsync(string? search, int page, int pageSize);
    Task<CriteriaDto?> GetByIdAsync(Guid id);
    Task<object> GetWithQuestionsAsync();
    Task<object> CreateAsync(CreateCriteriaRequest request);
    Task<(bool Success, string? Error)> UpdateAsync(Guid id, UpdateCriteriaRequest request);
    Task<(bool Success, string? Error)> DeleteAsync(Guid id);
}
