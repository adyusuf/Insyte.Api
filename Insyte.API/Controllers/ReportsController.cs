using Insyte.API.DTOs;
using Insyte.API.Services.Interfaces;
using Insyte.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insyte.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController : BaseController
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService) => _reportService = reportService;

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search, [FromQuery] ReportStatus? status,
        [FromQuery] Guid? schoolId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _reportService.GetAllAsync(search, status, schoolId, page, pageSize);
        return Ok(new ApiResponse<PagedResult<ReportDto>>(true, result));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var report = await _reportService.GetByIdAsync(id);
        if (report == null) return NotFound(new ApiError(false, "Rapor bulunamadı"));

        return Ok(new ApiResponse<ReportDto>(true, report));
    }

    [HttpGet("{id}/pdf")]
    public async Task<IActionResult> GetPdf(Guid id)
    {
        var (success, error, content, fileName) = await _reportService.GetPdfAsync(id);
        if (!success) return NotFound(new ApiError(false, error!));

        return File(content!, "application/pdf", fileName);
    }

    [HttpPost("{id}/send")]
    [Authorize(Policy = "AdminOrAdvisor")]
    public async Task<IActionResult> Send(Guid id)
    {
        var (success, error, result) = await _reportService.SendAsync(id, GetCurrentUserId());
        if (!success)
        {
            if (error == "Rapor bulunamadı") return NotFound(new ApiError(false, error));
            return BadRequest(new ApiError(false, error!));
        }

        return Ok(new ApiResponse<object>(true, result, "Rapor gönderim kuyruğuna eklendi"));
    }
}
