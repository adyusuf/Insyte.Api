using Insyte.API.DTOs;
using Insyte.API.Services.Interfaces;
using Insyte.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insyte.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VideosController : BaseController
{
    private readonly IVideoService _videoService;

    public VideosController(IVideoService videoService) => _videoService = videoService;

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search, [FromQuery] Guid? schoolId, [FromQuery] Guid? teacherId,
        [FromQuery] VideoStatus? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _videoService.GetAllAsync(search, schoolId, teacherId, status, page, pageSize);
        return Ok(new ApiResponse<PagedResult<VideoDto>>(true, result));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var video = await _videoService.GetByIdAsync(id);
        if (video == null) return NotFound(new ApiError(false, "Video bulunamadı"));

        return Ok(new ApiResponse<VideoDto>(true, video));
    }

    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] CreateVideoRequest request, IFormFile file)
    {
        var (success, error, result) = await _videoService.UploadAsync(request, file, GetCurrentUserId());
        if (!success) return BadRequest(new ApiError(false, error!));

        return Ok(new ApiResponse<object>(true, result, "Video yüklendi"));
    }

    [HttpPost("{id}/evaluate")]
    [Authorize(Policy = "AdminOrAdvisor")]
    public async Task<IActionResult> Evaluate(Guid id, [FromBody] EvaluateVideoRequest request)
    {
        var (success, error, result) = await _videoService.TriggerEvaluationAsync(id, request);
        if (!success) return NotFound(new ApiError(false, error!));

        return Ok(new ApiResponse<object>(true, result, "Değerlendirme başlatıldı"));
    }
}
