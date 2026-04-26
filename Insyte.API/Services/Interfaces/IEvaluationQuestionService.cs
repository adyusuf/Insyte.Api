using Insyte.API.DTOs;

namespace Insyte.API.Services.Interfaces;

public interface IEvaluationQuestionService
{
    Task<(bool CriteriaExists, List<EvaluationQuestionDto>? Questions)> GetAllAsync(Guid criteriaId);
    Task<EvaluationQuestionDto?> GetByIdAsync(Guid id, Guid criteriaId);
    Task<(bool Success, string? Error, object? Result)> CreateAsync(Guid criteriaId, CreateEvaluationQuestionRequest request);
    Task<(bool Success, string? Error)> UpdateAsync(Guid id, Guid criteriaId, UpdateEvaluationQuestionRequest request);
    Task<(bool Success, string? Error)> DeleteAsync(Guid id, Guid criteriaId);
}
