using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Insyte.Infrastructure.Data;
using Insyte.API.Controllers;
using Insyte.API.DTOs;
using Insyte.Core.Entities;
using Insyte.Core.Enums;
using System.Security.Claims;

namespace Insyte.API.Tests.Controllers;

public class CouncilsControllerTests : IDisposable
{
    private readonly InsyteDbContext _db;
    private readonly CouncilsController _controller;
    private readonly Guid _schoolId = Guid.NewGuid();
    private readonly Guid _userId = Guid.NewGuid();

    public CouncilsControllerTests()
    {
        var options = new DbContextOptionsBuilder<InsyteDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _db = new InsyteDbContext(options);

        var school = new School { Id = _schoolId, Name = "Test Okulu", IsActive = true, CreatedAt = DateTime.UtcNow };
        var user = new User { Id = _userId, Email = "test@okul.com", PasswordHash = "hash", FirstName = "Test", LastName = "Kullanıcı", Role = UserRole.SchoolAdmin, IsActive = true, CreatedAt = DateTime.UtcNow };
        var schoolUser = new SchoolUser { Id = Guid.NewGuid(), SchoolId = _schoolId, UserId = _userId, Role = UserRole.SchoolAdmin, AssignedAt = DateTime.UtcNow };
        _db.Schools.Add(school);
        _db.Users.Add(user);
        _db.SchoolUsers.Add(schoolUser);
        _db.SaveChanges();

        _controller = CreateController();
    }

    private CouncilsController CreateController()
    {
        var claims = new List<Claim>
        {
            new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", "test@okul.com"),
            new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "SchoolAdmin")
        };
        var httpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test")) };
        var accessor = new HttpContextAccessor { HttpContext = httpContext };
        return new CouncilsController(_db, accessor);
    }

    // ─── Council Entity Tests ─────────────────────────────────────────────────

    [Fact]
    public void Council_ValidEntity_ShouldHaveCorrectProperties()
    {
        var council = new Council
        {
            Id = Guid.NewGuid(),
            Name = "Okul Yönetim Kurulu",
            SchoolId = _schoolId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        council.Name.Should().Be("Okul Yönetim Kurulu");
        council.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Council_SoftDelete_ShouldSetInactiveAndPreserveData()
    {
        var council = new Council { Name = "Disiplin Kurulu", SchoolId = _schoolId, IsActive = true, CreatedAt = DateTime.UtcNow };
        council.IsActive = false;
        council.UpdatedAt = DateTime.UtcNow;

        council.IsActive.Should().BeFalse();
        council.Name.Should().Be("Disiplin Kurulu");
    }

    // ─── Controller Integration Tests (InMemory DB) ──────────────────────────

    [Fact]
    public async Task GetAll_WithValidSchoolUser_ReturnsOkWithCouncils()
    {
        _db.Councils.Add(new Council { Id = Guid.NewGuid(), Name = "Test Kurulu", SchoolId = _schoolId, IsActive = true, CreatedAt = DateTime.UtcNow });
        await _db.SaveChangesAsync();

        var result = await _controller.GetAll(null, 1, 20);

        result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result).StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task Create_WithValidData_ReturnsOkAndPersists()
    {
        var dto = new CreateCouncilRequest("Yeni Kurul", "Açıklama");

        var result = await _controller.Create(dto);

        result.Should().BeOfType<CreatedAtActionResult>();
        _db.Councils.Should().ContainSingle(c => c.Name == "Yeni Kurul");
    }

    [Fact]
    public async Task Update_ExistingCouncil_UpdatesNameAndDescription()
    {
        var councilId = Guid.NewGuid();
        _db.Councils.Add(new Council { Id = councilId, Name = "Eski Ad", SchoolId = _schoolId, IsActive = true, CreatedAt = DateTime.UtcNow });
        await _db.SaveChangesAsync();

        var dto = new UpdateCouncilRequest("Yeni Ad", "Yeni Açıklama", null);
        var result = await _controller.Update(councilId, dto);

        result.Should().BeOfType<OkObjectResult>();
        var updated = await _db.Councils.FindAsync(councilId);
        updated!.Name.Should().Be("Yeni Ad");
    }

    [Fact]
    public async Task Update_NonExistingCouncil_ReturnsNotFound()
    {
        var dto = new UpdateCouncilRequest("Yeni Ad", null, null);
        var result = await _controller.Update(Guid.NewGuid(), dto);
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Delete_ExistingCouncil_SoftDeletesSuccessfully()
    {
        var councilId = Guid.NewGuid();
        _db.Councils.Add(new Council { Id = councilId, Name = "Silinecek Kurul", SchoolId = _schoolId, IsActive = true, CreatedAt = DateTime.UtcNow });
        await _db.SaveChangesAsync();

        var result = await _controller.Delete(councilId);

        result.Should().BeOfType<OkObjectResult>();
        (await _db.Councils.FindAsync(councilId))!.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task AddMember_ValidUser_AddsMemberToCouncil()
    {
        var councilId = Guid.NewGuid();
        _db.Councils.Add(new Council { Id = councilId, Name = "Kurul", SchoolId = _schoolId, IsActive = true, CreatedAt = DateTime.UtcNow });
        await _db.SaveChangesAsync();

        var result = await _controller.AddMember(councilId, new AddCouncilMemberRequest(_userId, "Üye"));

        result.Should().BeOfType<OkObjectResult>();
        _db.CouncilMembers.Should().ContainSingle(m => m.CouncilId == councilId && m.UserId == _userId);
    }

    [Fact]
    public async Task GetMembers_ExistingCouncil_ReturnsMembers()
    {
        var councilId = Guid.NewGuid();
        _db.Councils.Add(new Council { Id = councilId, Name = "Kurul", SchoolId = _schoolId, IsActive = true, CreatedAt = DateTime.UtcNow });
        _db.CouncilMembers.Add(new CouncilMember { Id = Guid.NewGuid(), CouncilId = councilId, UserId = _userId, Role = "Üye", AssignedAt = DateTime.UtcNow });
        await _db.SaveChangesAsync();

        var result = await _controller.GetMembers(councilId);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task RemoveMember_ExistingMember_RemovesSuccessfully()
    {
        var councilId = Guid.NewGuid();
        var memberId = Guid.NewGuid();
        _db.Councils.Add(new Council { Id = councilId, Name = "Kurul", SchoolId = _schoolId, IsActive = true, CreatedAt = DateTime.UtcNow });
        _db.CouncilMembers.Add(new CouncilMember { Id = memberId, CouncilId = councilId, UserId = _userId, Role = "Üye", AssignedAt = DateTime.UtcNow });
        await _db.SaveChangesAsync();

        var result = await _controller.RemoveMember(councilId, memberId);

        result.Should().BeOfType<OkObjectResult>();
        (await _db.CouncilMembers.FindAsync(memberId)).Should().BeNull();
    }

    public void Dispose() => _db.Dispose();
}
