using Insyte.API.DTOs;
using Insyte.API.Services.Interfaces;
using Insyte.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insyte.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EvaluationsController : BaseController
{
    private readonly IEvaluationService _evaluationService;

    public EvaluationsController(IEvaluationService evaluationService) => _evaluationService = evaluationService;

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search, [FromQuery] EvaluationStatus? status,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _evaluationService.GetAllAsync(search, status, page, pageSize);
        return Ok(new ApiResponse<PagedResult<EvaluationDto>>(true, result));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var evaluation = await _evaluationService.GetByIdAsync(id);
        if (evaluation == null) return NotFound(new ApiError(false, "Değerlendirme bulunamadı"));

        return Ok(new ApiResponse<EvaluationDto>(true, evaluation));
    }

    [HttpPut("{id}/approve")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Approve(Guid id)
    {
        var (success, error, result) = await _evaluationService.ApproveAsync(id, GetCurrentUserId());
        if (!success)
        {
            if (error == "Değerlendirme bulunamadı") return NotFound(new ApiError(false, error));
            return BadRequest(new ApiError(false, error!));
        }

        return Ok(new ApiResponse<object>(true, result, "Değerlendirme onaylandı, rapor oluşturuldu"));
    }

    [HttpPut("{id}/reject")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Reject(Guid id)
    {
        var (success, error) = await _evaluationService.RejectAsync(id);
        if (!success) return NotFound(new ApiError(false, error!));

        return Ok(new ApiResponse<object>(true, null, "Değerlendirme reddedildi"));
    }
}
