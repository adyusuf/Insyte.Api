using Insyte.API.DTOs;
using Insyte.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insyte.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CriteriaController : BaseController
{
    private readonly ICriteriaService _criteriaService;

    public CriteriaController(ICriteriaService criteriaService) => _criteriaService = criteriaService;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _criteriaService.GetAllAsync(search, page, pageSize);
        return Ok(new ApiResponse<PagedResult<CriteriaDto>>(true, result));
    }

    [HttpGet("with-questions")]
    public async Task<IActionResult> GetWithQuestions()
    {
        var result = await _criteriaService.GetWithQuestionsAsync();
        return Ok(new ApiResponse<object>(true, result));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var criteria = await _criteriaService.GetByIdAsync(id);
        if (criteria == null) return NotFound(new ApiError(false, "Kriter bulunamadı"));

        return Ok(new ApiResponse<CriteriaDto>(true, criteria));
    }

    [HttpPost]
    [Authorize(Policy = "AdminOrAdvisor")]
    public async Task<IActionResult> Create([FromBody] CreateCriteriaRequest request)
    {
        var result = await _criteriaService.CreateAsync(request);
        return Ok(new ApiResponse<object>(true, result, "Kriter oluşturuldu"));
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOrAdvisor")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCriteriaRequest request)
    {
        var (success, error) = await _criteriaService.UpdateAsync(id, request);
        if (!success) return NotFound(new ApiError(false, error!));

        return Ok(new ApiResponse<object>(true, null, "Kriter güncellendi"));
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var (success, error) = await _criteriaService.DeleteAsync(id);
        if (!success) return NotFound(new ApiError(false, error!));

        return Ok(new ApiResponse<object>(true, null, "Kriter deaktif edildi"));
    }
}
