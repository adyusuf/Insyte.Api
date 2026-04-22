using System.Security.Claims;
using Insyte.API.DTOs;
using Insyte.Core.Entities;
using Insyte.Core.Enums;
using Insyte.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Insyte.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EvaluationsController : ControllerBase
{
    private readonly InsyteDbContext _db;

    public EvaluationsController(InsyteDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] EvaluationStatus? status,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var query = _db.Evaluations
            .Include(e => e.Video)
            .Include(e => e.Criteria)
            .Include(e => e.AIModel)
            .AsQueryable();

        if (!string.IsNullOrEmpty(search))
            query = query.Where(e => e.Video.Title.Contains(search) || e.Criteria.Name.Contains(search));
        if (status.HasValue)
            query = query.Where(e => e.Status == status.Value);

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(e => e.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new EvaluationDto(
                e.Id, e.VideoId, e.Video.Title, e.CriteriaId, e.Criteria.Name,
                e.AIModelId, e.AIModel.Name, e.Result,
                e.TokenUsageInput, e.TokenUsageOutput, e.Status, e.ErrorMessage,
                e.CreatedAt, e.CompletedAt))
            .ToListAsync();

        return Ok(new ApiResponse<PagedResult<EvaluationDto>>(true, new PagedResult<EvaluationDto>(items, total, page, pageSize)));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var e = await _db.Evaluations
            .Include(e => e.Video)
            .Include(e => e.Criteria)
            .Include(e => e.AIModel)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (e == null) return NotFound(new ApiError(false, "Değerlendirme bulunamadı"));

        var dto = new EvaluationDto(
            e.Id, e.VideoId, e.Video.Title, e.CriteriaId, e.Criteria.Name,
            e.AIModelId, e.AIModel.Name, e.Result,
            e.TokenUsageInput, e.TokenUsageOutput, e.Status, e.ErrorMessage,
            e.CreatedAt, e.CompletedAt);

        return Ok(new ApiResponse<EvaluationDto>(true, dto));
    }

    [HttpPut("{id}/approve")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Approve(Guid id)
    {
        var evaluation = await _db.Evaluations.Include(e => e.Video).FirstOrDefaultAsync(e => e.Id == id);
        if (evaluation == null) return NotFound(new ApiError(false, "Değerlendirme bulunamadı"));

        if (evaluation.Status != EvaluationStatus.Completed)
            return BadRequest(new ApiError(false, "Sadece tamamlanmış değerlendirmeler onaylanabilir"));

        evaluation.Video.Status = VideoStatus.Approved;

        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var report = new Report
        {
            EvaluationId = id,
            ApprovedByUserId = userId,
            ApprovedAt = DateTime.UtcNow,
            Status = ReportStatus.Approved
        };

        _db.Reports.Add(report);
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<object>(true, new { report.Id }, "Değerlendirme onaylandı, rapor oluşturuldu"));
    }

    [HttpPut("{id}/reject")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Reject(Guid id)
    {
        var evaluation = await _db.Evaluations.Include(e => e.Video).FirstOrDefaultAsync(e => e.Id == id);
        if (evaluation == null) return NotFound(new ApiError(false, "Değerlendirme bulunamadı"));

        evaluation.Video.Status = VideoStatus.Rejected;
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<object>(true, null, "Değerlendirme reddedildi"));
    }
}
