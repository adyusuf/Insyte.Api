using Insyte.API.DTOs;
using Insyte.Core.Enums;

namespace Insyte.API.Services.Interfaces;

public interface IEvaluationService
{
    Task<PagedResult<EvaluationDto>> GetAllAsync(string? search, EvaluationStatus? status, int page, int pageSize);
    Task<EvaluationDto?> GetByIdAsync(Guid id);
    Task<(bool Success, string? Error, object? Result)> ApproveAsync(Guid id, Guid approverId);
    Task<(bool Success, string? Error)> RejectAsync(Guid id);
}
