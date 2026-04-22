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
public class VideosController : ControllerBase
{
    private readonly InsyteDbContext _db;
    private readonly IConfiguration _config;

    public VideosController(InsyteDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] Guid? schoolId, [FromQuery] Guid? teacherId,
        [FromQuery] VideoStatus? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var query = _db.Videos.Include(v => v.School).Include(v => v.Teacher).AsQueryable();

        if (!string.IsNullOrEmpty(search))
            query = query.Where(v => v.Title.Contains(search));
        if (schoolId.HasValue)
            query = query.Where(v => v.SchoolId == schoolId.Value);
        if (teacherId.HasValue)
            query = query.Where(v => v.TeacherUserId == teacherId.Value);
        if (status.HasValue)
            query = query.Where(v => v.Status == status.Value);

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(v => v.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(v => new VideoDto(
                v.Id, v.Title, v.OriginalFileName, v.FileSize, v.SchoolId, v.School.Name,
                v.TeacherUserId, v.Teacher.FirstName + " " + v.Teacher.LastName,
                v.Subject, v.Status, v.CreatedAt, v.Evaluations.Count))
            .ToListAsync();

        return Ok(new ApiResponse<PagedResult<VideoDto>>(true, new PagedResult<VideoDto>(items, total, page, pageSize)));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var v = await _db.Videos
            .Include(v => v.School)
            .Include(v => v.Teacher)
            .Include(v => v.Evaluations)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (v == null) return NotFound(new ApiError(false, "Video bulunamadı"));

        var dto = new VideoDto(
            v.Id, v.Title, v.OriginalFileName, v.FileSize, v.SchoolId, v.School.Name,
            v.TeacherUserId, v.Teacher.FirstName + " " + v.Teacher.LastName,
            v.Subject, v.Status, v.CreatedAt, v.Evaluations.Count);

        return Ok(new ApiResponse<VideoDto>(true, dto));
    }

    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] CreateVideoRequest request, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new ApiError(false, "Video dosyası gerekli"));

        var maxSize = int.Parse(_config["Upload:MaxFileSizeMB"]!) * 1024L * 1024L;
        if (file.Length > maxSize)
            return BadRequest(new ApiError(false, $"Dosya boyutu {_config["Upload:MaxFileSizeMB"]}MB'dan büyük olamaz"));

        var uploadPath = _config["Upload:VideoPath"]!;
        Directory.CreateDirectory(uploadPath);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(uploadPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var video = new Video
        {
            Title = request.Title,
            FilePath = filePath,
            OriginalFileName = file.FileName,
            FileSize = file.Length,
            SchoolId = request.SchoolId,
            TeacherUserId = request.TeacherUserId,
            UploadedByUserId = userId,
            Subject = request.Subject
        };

        _db.Videos.Add(video);
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<object>(true, new { video.Id }, "Video yüklendi"));
    }

    [HttpPost("{id}/evaluate")]
    [Authorize(Policy = "AdminOrAdvisor")]
    public async Task<IActionResult> Evaluate(Guid id, [FromBody] EvaluateVideoRequest request)
    {
        var video = await _db.Videos.FindAsync(id);
        if (video == null) return NotFound(new ApiError(false, "Video bulunamadı"));

        var evaluation = new Evaluation
        {
            VideoId = id,
            CriteriaId = request.CriteriaId,
            AIModelId = request.AIModelId,
            Status = EvaluationStatus.Pending
        };

        video.Status = VideoStatus.Processing;

        _db.Evaluations.Add(evaluation);
        await _db.SaveChangesAsync();

        // TODO: Trigger AI evaluation in background service
        return Ok(new ApiResponse<object>(true, new { evaluation.Id }, "Değerlendirme başlatıldı"));
    }
}
