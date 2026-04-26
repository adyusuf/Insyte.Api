using Insyte.API.DTOs;
using Insyte.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insyte.API.Controllers;

[ApiController]
[Route("api/ai-providers")]
[Authorize(Policy = "AdminOnly")]
public class AIProvidersController : BaseController
{
    private readonly IAIConfigService _aiConfigService;

    public AIProvidersController(IAIConfigService aiConfigService) => _aiConfigService = aiConfigService;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _aiConfigService.GetAllProvidersAsync();
        return Ok(new ApiResponse<List<AIProviderDto>>(true, items));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var provider = await _aiConfigService.GetProviderByIdAsync(id);
        if (provider == null) return NotFound(new ApiError(false, "AI Provider bulunamadı"));

        return Ok(new ApiResponse<AIProviderDto>(true, provider));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAIProviderRequest request)
    {
        var result = await _aiConfigService.CreateProviderAsync(request);
        return Ok(new ApiResponse<object>(true, result, "AI Provider oluşturuldu"));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAIProviderRequest request)
    {
        var (success, error) = await _aiConfigService.UpdateProviderAsync(id, request);
        if (!success) return NotFound(new ApiError(false, error!));

        return Ok(new ApiResponse<object>(true, null, "AI Provider güncellendi"));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var (success, error) = await _aiConfigService.DeleteProviderAsync(id);
        if (!success) return NotFound(new ApiError(false, error!));

        return Ok(new ApiResponse<object>(true, null, "AI Provider deaktif edildi"));
    }

    // --- Models ---
    [HttpGet("{id}/models")]
    public async Task<IActionResult> GetModels(Guid id)
    {
        var models = await _aiConfigService.GetModelsAsync(id);
        return Ok(new ApiResponse<List<AIModelDto>>(true, models));
    }

    [HttpPost("{id}/models")]
    public async Task<IActionResult> CreateModel(Guid id, [FromBody] CreateAIModelRequest request)
    {
        var (success, error, result) = await _aiConfigService.CreateModelAsync(id, request);
        if (!success) return NotFound(new ApiError(false, error!));

        return Ok(new ApiResponse<object>(true, result, "Model oluşturuldu"));
    }
}

[ApiController]
[Route("api/ai-models")]
[Authorize(Policy = "AdminOnly")]
public class AIModelsController : BaseController
{
    private readonly IAIConfigService _aiConfigService;

    public AIModelsController(IAIConfigService aiConfigService) => _aiConfigService = aiConfigService;

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var models = await _aiConfigService.GetAllActiveModelsAsync();
        return Ok(new ApiResponse<List<AIModelDto>>(true, models));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAIModelRequest request)
    {
        var (success, error) = await _aiConfigService.UpdateModelAsync(id, request);
        if (!success) return NotFound(new ApiError(false, error!));

        return Ok(new ApiResponse<object>(true, null, "Model güncellendi"));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var (success, error) = await _aiConfigService.DeleteModelAsync(id);
        if (!success) return NotFound(new ApiError(false, error!));

        return Ok(new ApiResponse<object>(true, null, "Model deaktif edildi"));
    }
}
