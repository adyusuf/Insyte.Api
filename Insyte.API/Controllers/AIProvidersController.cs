using Insyte.API.DTOs;
using Insyte.Core.Entities;
using Insyte.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Insyte.API.Controllers;

[ApiController]
[Route("api/ai-providers")]
[Authorize(Policy = "AdminOnly")]
public class AIProvidersController : ControllerBase
{
    private readonly InsyteDbContext _db;

    public AIProvidersController(InsyteDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _db.AIProviders
            .Include(p => p.Models)
            .OrderBy(p => p.Name)
            .Select(p => new AIProviderDto(p.Id, p.Name, p.Provider, p.BaseUrl, p.IsActive, p.CreatedAt, p.Models.Count))
            .ToListAsync();

        return Ok(new ApiResponse<List<AIProviderDto>>(true, items));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var p = await _db.AIProviders.Include(p => p.Models).FirstOrDefaultAsync(p => p.Id == id);
        if (p == null) return NotFound(new ApiError(false, "AI Provider bulunamadı"));

        return Ok(new ApiResponse<AIProviderDto>(true,
            new AIProviderDto(p.Id, p.Name, p.Provider, p.BaseUrl, p.IsActive, p.CreatedAt, p.Models.Count)));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAIProviderRequest request)
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

        return Ok(new ApiResponse<object>(true, new { provider.Id }, "AI Provider oluşturuldu"));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAIProviderRequest request)
    {
        var provider = await _db.AIProviders.FindAsync(id);
        if (provider == null) return NotFound(new ApiError(false, "AI Provider bulunamadı"));

        if (request.Name != null) provider.Name = request.Name;
        if (request.Provider != null) provider.Provider = request.Provider;
        if (request.ApiKey != null) provider.ApiKey = request.ApiKey;
        if (request.BaseUrl != null) provider.BaseUrl = request.BaseUrl;
        if (request.IsActive.HasValue) provider.IsActive = request.IsActive.Value;

        await _db.SaveChangesAsync();
        return Ok(new ApiResponse<object>(true, null, "AI Provider güncellendi"));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var provider = await _db.AIProviders.FindAsync(id);
        if (provider == null) return NotFound(new ApiError(false, "AI Provider bulunamadı"));

        provider.IsActive = false;
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<object>(true, null, "AI Provider deaktif edildi"));
    }

    // --- Models ---
    [HttpGet("{id}/models")]
    public async Task<IActionResult> GetModels(Guid id)
    {
        var models = await _db.AIModels
            .Where(m => m.AIProviderId == id)
            .Include(m => m.AIProvider)
            .Select(m => new AIModelDto(m.Id, m.AIProviderId, m.AIProvider.Name, m.Name, m.ModelId, m.MaxTokens, m.IsActive, m.CreatedAt))
            .ToListAsync();

        return Ok(new ApiResponse<List<AIModelDto>>(true, models));
    }

    [HttpPost("{id}/models")]
    public async Task<IActionResult> CreateModel(Guid id, [FromBody] CreateAIModelRequest request)
    {
        if (!await _db.AIProviders.AnyAsync(p => p.Id == id))
            return NotFound(new ApiError(false, "AI Provider bulunamadı"));

        var model = new AIModel
        {
            AIProviderId = id,
            Name = request.Name,
            ModelId = request.ModelId,
            MaxTokens = request.MaxTokens
        };

        _db.AIModels.Add(model);
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<object>(true, new { model.Id }, "Model oluşturuldu"));
    }
}

[ApiController]
[Route("api/ai-models")]
[Authorize(Policy = "AdminOnly")]
public class AIModelsController : ControllerBase
{
    private readonly InsyteDbContext _db;

    public AIModelsController(InsyteDbContext db) => _db = db;

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var models = await _db.AIModels
            .Include(m => m.AIProvider)
            .Where(m => m.IsActive && m.AIProvider.IsActive)
            .Select(m => new AIModelDto(m.Id, m.AIProviderId, m.AIProvider.Name, m.Name, m.ModelId, m.MaxTokens, m.IsActive, m.CreatedAt))
            .ToListAsync();

        return Ok(new ApiResponse<List<AIModelDto>>(true, models));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAIModelRequest request)
    {
        var model = await _db.AIModels.FindAsync(id);
        if (model == null) return NotFound(new ApiError(false, "Model bulunamadı"));

        if (request.Name != null) model.Name = request.Name;
        if (request.ModelId != null) model.ModelId = request.ModelId;
        if (request.MaxTokens.HasValue) model.MaxTokens = request.MaxTokens.Value;
        if (request.IsActive.HasValue) model.IsActive = request.IsActive.Value;

        await _db.SaveChangesAsync();
        return Ok(new ApiResponse<object>(true, null, "Model güncellendi"));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var model = await _db.AIModels.FindAsync(id);
        if (model == null) return NotFound(new ApiError(false, "Model bulunamadı"));

        model.IsActive = false;
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<object>(true, null, "Model deaktif edildi"));
    }
}
