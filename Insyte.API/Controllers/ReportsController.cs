using Insyte.API.DTOs;
using Insyte.Core.Enums;
using Insyte.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Insyte.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly InsyteDbContext _db;

    public ReportsController(InsyteDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] ReportStatus? status,
        [FromQuery] Guid? schoolId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var query = _db.Reports
            .Include(r => r.Evaluation).ThenInclude(e => e.Video).ThenInclude(v => v.School)
            .Include(r => r.Evaluation).ThenInclude(e => e.Video).ThenInclude(v => v.Teacher)
            .Include(r => r.ApprovedBy)
            .AsQueryable();

        if (!string.IsNullOrEmpty(search))
            query = query.Where(r => r.Evaluation.Video.Title.Contains(search) ||
                                     r.Evaluation.Video.School.Name.Contains(search));
        if (status.HasValue)
            query = query.Where(r => r.Status == status.Value);
        if (schoolId.HasValue)
            query = query.Where(r => r.Evaluation.Video.SchoolId == schoolId.Value);

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new ReportDto(
                r.Id, r.EvaluationId, r.Evaluation.Video.Title,
                r.Evaluation.Video.School.Name,
                r.Evaluation.Video.Teacher.FirstName + " " + r.Evaluation.Video.Teacher.LastName,
                r.PdfPath,
                r.ApprovedBy != null ? r.ApprovedBy.FirstName + " " + r.ApprovedBy.LastName : null,
                r.ApprovedAt, r.Status, r.CreatedAt))
            .ToListAsync();

        return Ok(new ApiResponse<PagedResult<ReportDto>>(true, new PagedResult<ReportDto>(items, total, page, pageSize)));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var r = await _db.Reports
            .Include(r => r.Evaluation).ThenInclude(e => e.Video).ThenInclude(v => v.School)
            .Include(r => r.Evaluation).ThenInclude(e => e.Video).ThenInclude(v => v.Teacher)
            .Include(r => r.ApprovedBy)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (r == null) return NotFound(new ApiError(false, "Rapor bulunamadı"));

        var dto = new ReportDto(
            r.Id, r.EvaluationId, r.Evaluation.Video.Title,
            r.Evaluation.Video.School.Name,
            r.Evaluation.Video.Teacher.FirstName + " " + r.Evaluation.Video.Teacher.LastName,
            r.PdfPath,
            r.ApprovedBy != null ? r.ApprovedBy.FirstName + " " + r.ApprovedBy.LastName : null,
            r.ApprovedAt, r.Status, r.CreatedAt);

        return Ok(new ApiResponse<ReportDto>(true, dto));
    }

    [HttpGet("{id}/pdf")]
    public async Task<IActionResult> GetPdf(Guid id)
    {
        var report = await _db.Reports.FindAsync(id);
        if (report == null) return NotFound(new ApiError(false, "Rapor bulunamadı"));
        if (string.IsNullOrEmpty(report.PdfPath) || !System.IO.File.Exists(report.PdfPath))
            return NotFound(new ApiError(false, "PDF dosyası bulunamadı"));

        var bytes = await System.IO.File.ReadAllBytesAsync(report.PdfPath);
        return File(bytes, "application/pdf", $"report-{report.Id}.pdf");
    }

    [HttpPost("{id}/send")]
    [Authorize(Policy = "AdminOrAdvisor")]
    public async Task<IActionResult> Send(Guid id)
    {
        var report = await _db.Reports
            .Include(r => r.Evaluation).ThenInclude(e => e.Video)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (report == null) return NotFound(new ApiError(false, "Rapor bulunamadı"));
        if (report.Status != ReportStatus.Approved)
            return BadRequest(new ApiError(false, "Sadece onaylanmış raporlar gönderilebilir"));

        // Get email configs for the school
        var emailConfigs = await _db.EmailConfigs
            .Where(ec => ec.SchoolId == report.Evaluation.Video.SchoolId && ec.IsActive)
            .ToListAsync();

        // TODO: Actually send emails via email service
        foreach (var config in emailConfigs)
        {
            _db.EmailLogs.Add(new Core.Entities.EmailLog
            {
                ReportId = id,
                RecipientEmail = config.RecipientEmail,
                Status = EmailStatus.Pending
            });
        }

        report.Status = ReportStatus.Sent;
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<object>(true, new { EmailCount = emailConfigs.Count }, "Rapor gönderim kuyruğuna eklendi"));
    }
}
