using Insyte.API.DTOs;
using Insyte.API.Services.Interfaces;
using Insyte.Core.Entities;
using Insyte.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Insyte.API.Services;

public class AIConfigService : IAIConfigService
{
    private readonly InsyteDbContext _db;

    public AIConfigService(InsyteDbContext db) => _db = db;

    // Providers
    public async Task<List<AIProviderDto>> GetAllProvidersAsync()
    {
        return await _db.AIProviders
            .Include(p => p.Models)
            .OrderBy(p => p.Name)
            .Select(p => new AIProviderDto(p.Id, p.Name, p.Provider, p.BaseUrl, p.IsActive, p.CreatedAt, p.Models.Count))
            .ToListAsync();
    }

    public async Task<AIProviderDto?> GetProviderByIdAsync(Guid id)
    {
        var p = await _db.AIProviders.Include(p => p.Models).FirstOrDefaultAsync(p => p.Id == id);
        if (p == null) return null;

        return new AIProviderDto(p.Id, p.Name, p.Provider, p.BaseUrl, p.IsActive, p.CreatedAt, p.Models.Count);
    }

    public async Task<object> CreateProviderAsync(CreateAIProviderRequest request)
    {
        var provider = new AIProvider
        {
            Name = request.Name,
            Provider = request.Provider,
            ApiKey = request.ApiKey,
            BaseUrl = request.BaseUrl
        };

        _db.AIProviders.Add(provider);
        await _db.SaveChangesAsync();

        return new { provider.Id };
    }

    public async Task<(bool Success, string? Error)> UpdateProviderAsync(Guid id, UpdateAIProviderRequest request)
    {
        var provider = await _db.AIProviders.FindAsync(id);
        if (provider == null) return (false, "AI Provider bulunamadı");

        if (request.Name != null) provider.Name = request.Name;
        if (request.Provider != null) provider.Provider = request.Provider;
        if (request.ApiKey != null) provider.ApiKey = request.ApiKey;
        if (request.BaseUrl != null) provider.BaseUrl = request.BaseUrl;
        if (request.IsActive.HasValue) provider.IsActive = request.IsActive.Value;

        await _db.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> DeleteProviderAsync(Guid id)
    {
        var provider = await _db.AIProviders.FindAsync(id);
        if (provider == null) return (false, "AI Provider bulunamadı");

        provider.IsActive = false;
        await _db.SaveChangesAsync();

        return (true, null);
    }

    // Models
    public async Task<List<AIModelDto>> GetModelsAsync(Guid providerId)
    {
        return await _db.AIModels
            .Where(m => m.AIProviderId == providerId)
            .Include(m => m.AIProvider)
            .Select(m => new AIModelDto(m.Id, m.AIProviderId, m.AIProvider.Name, m.Name, m.ModelId, m.MaxTokens, m.IsActive, m.CreatedAt))
            .ToListAsync();
    }

    public async Task<List<AIModelDto>> GetAllActiveModelsAsync()
    {
        return await _db.AIModels
            .Include(m => m.AIProvider)
            .Where(m => m.IsActive && m.AIProvider.IsActive)
            .Select(m => new AIModelDto(m.Id, m.AIProviderId, m.AIProvider.Name, m.Name, m.ModelId, m.MaxTokens, m.IsActive, m.CreatedAt))
            .ToListAsync();
    }

    public async Task<(bool Success, string? Error, object? Result)> CreateModelAsync(Guid providerId, CreateAIModelRequest request)
    {
        if (!await _db.AIProviders.AnyAsync(p => p.Id == providerId))
            return (false, "AI Provider bulunamadı", null);

        var model = new AIModel
        {
            AIProviderId = providerId,
            Name = request.Name,
            ModelId = request.ModelId,
            MaxTokens = request.MaxTokens
        };

        _db.AIModels.Add(model);
        await _db.SaveChangesAsync();

        return (true, null, new { model.Id });
    }

    public async Task<(bool Success, string? Error)> UpdateModelAsync(Guid id, UpdateAIModelRequest request)
    {
        var model = await _db.AIModels.FindAsync(id);
        if (model == null) return (false, "Model bulunamadı");

        if (request.Name != null) model.Name = request.Name;
        if (request.ModelId != null) model.ModelId = request.ModelId;
        if (request.MaxTokens.HasValue) model.MaxTokens = request.MaxTokens.Value;
        if (request.IsActive.HasValue) model.IsActive = request.IsActive.Value;

        await _db.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> DeleteModelAsync(Guid id)
    {
        var model = await _db.AIModels.FindAsync(id);
        if (model == null) return (false, "Model bulunamadı");

        model.IsActive = false;
        await _db.SaveChangesAsync();

        return (true, null);
    }
}
