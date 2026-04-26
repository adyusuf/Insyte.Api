using Insyte.API.DTOs;
using Insyte.API.Services.Interfaces;
using Insyte.Core.Entities;
using Insyte.Core.Enums;
using Insyte.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Insyte.API.Services;

public class VideoService : IVideoService
{
    private readonly InsyteDbContext _db;
    private readonly IConfiguration _config;

    public VideoService(InsyteDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    public async Task<PagedResult<VideoDto>> GetAllAsync(string? search, Guid? schoolId, Guid? teacherId, VideoStatus? status, int page, int pageSize)
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

        return new PagedResult<VideoDto>(items, total, page, pageSize);
    }

    public async Task<VideoDto?> GetByIdAsync(Guid id)
    {
        var v = await _db.Videos
            .Include(v => v.School)
            .Include(v => v.Teacher)
            .Include(v => v.Evaluations)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (v == null) return null;

        return new VideoDto(
            v.Id, v.Title, v.OriginalFileName, v.FileSize, v.SchoolId, v.School.Name,
            v.TeacherUserId, v.Teacher.FirstName + " " + v.Teacher.LastName,
            v.Subject, v.Status, v.CreatedAt, v.Evaluations.Count);
    }

    public async Task<(bool Success, string? Error, object? Result)> UploadAsync(CreateVideoRequest request, IFormFile file, Guid uploadedByUserId)
    {
        if (file == null || file.Length == 0)
            return (false, "Video dosyası gerekli", null);

        var maxSize = long.Parse(_config["Upload:MaxFileSizeMB"]!) * 1024L * 1024L;
        if (file.Length > maxSize)
            return (false, $"Dosya boyutu {_config["Upload:MaxFileSizeMB"]}MB'dan büyük olamaz", null);

        var uploadPath = _config["Upload:VideoPath"]!;
        Directory.CreateDirectory(uploadPath);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(uploadPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var video = new Video
        {
            Title = request.Title,
            FilePath = filePath,
            OriginalFileName = file.FileName,
            FileSize = file.Length,
            SchoolId = request.SchoolId,
            TeacherUserId = request.TeacherUserId,
            UploadedByUserId = uploadedByUserId,
            Subject = request.Subject
        };

        _db.Videos.Add(video);
        await _db.SaveChangesAsync();

        return (true, null, new { video.Id });
    }

    public async Task<(bool Success, string? Error, object? Result)> TriggerEvaluationAsync(Guid videoId, EvaluateVideoRequest request)
    {
        var video = await _db.Videos.FindAsync(videoId);
        if (video == null) return (false, "Video bulunamadı", null);

        var evaluation = new Evaluation
        {
            VideoId = videoId,
            CriteriaId = request.CriteriaId,
            AIModelId = request.AIModelId,
            Status = EvaluationStatus.Pending
        };

        video.Status = VideoStatus.Processing;

        _db.Evaluations.Add(evaluation);
        await _db.SaveChangesAsync();

        // TODO: Hangfire ile arka plan işi kuyruğa eklenecek
        return (true, null, new { evaluation.Id });
    }
}
