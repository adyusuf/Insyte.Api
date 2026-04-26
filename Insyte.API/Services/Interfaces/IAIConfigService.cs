using Insyte.API.DTOs;

namespace Insyte.API.Services.Interfaces;

public interface IAIConfigService
{
    // Providers
    Task<List<AIProviderDto>> GetAllProvidersAsync();
    Task<AIProviderDto?> GetProviderByIdAsync(Guid id);
    Task<object> CreateProviderAsync(CreateAIProviderRequest request);
    Task<(bool Success, string? Error)> UpdateProviderAsync(Guid id, UpdateAIProviderRequest request);
    Task<(bool Success, string? Error)> DeleteProviderAsync(Guid id);

    // Models
    Task<List<AIModelDto>> GetModelsAsync(Guid providerId);
    Task<List<AIModelDto>> GetAllActiveModelsAsync();
    Task<(bool Success, string? Error, object? Result)> CreateModelAsync(Guid providerId, CreateAIModelRequest request);
    Task<(bool Success, string? Error)> UpdateModelAsync(Guid id, UpdateAIModelRequest request);
    Task<(bool Success, string? Error)> DeleteModelAsync(Guid id);
}
