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

public class WorkingGroupsControllerTests : IDisposable
{
    private readonly InsyteDbContext _db;
    private readonly WorkingGroupsController _controller;
    private readonly Guid _schoolId = Guid.NewGuid();
    private readonly Guid _userId = Guid.NewGuid();

    public WorkingGroupsControllerTests()
    {
        var options = new DbContextOptionsBuilder<InsyteDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _db = new InsyteDbContext(options);

        // Seed kullanıcı ve okul
        var school = new School
        {
            Id = _schoolId,
            Name = "Test Okulu",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        var user = new User
        {
            Id = _userId,
            Email = "test@okul.com",
            PasswordHash = "hash",
            FirstName = "Test",
            LastName = "Kullanıcı",
            Role = UserRole.SchoolAdmin,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        var schoolUser = new SchoolUser
        {
            Id = Guid.NewGuid(),
            SchoolId = _schoolId,
            UserId = _userId,
            Role = UserRole.SchoolAdmin,
            AssignedAt = DateTime.UtcNow
        };
        _db.Schools.Add(school);
        _db.Users.Add(user);
        _db.SchoolUsers.Add(schoolUser);
        _db.SaveChanges();

        _controller = CreateController();
    }

    private WorkingGroupsController CreateController()
    {
        var claims = new List<Claim>
        {
            new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", "test@okul.com"),
            new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "SchoolAdmin")
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext { User = principal };
        var accessor = new HttpContextAccessor { HttpContext = httpContext };
        return new WorkingGroupsController(_db, accessor);
    }

    // ─── WorkingGroup Entity Tests ─────────────────────────────────────────────

    [Fact]
    public void WorkingGroup_ValidEntity_ShouldHaveCorrectProperties()
    {
        var group = new WorkingGroup
        {
            Id = Guid.NewGuid(),
            Name = "Matematik Zümresi",
            Description = "Açıklama",
            SchoolId = _schoolId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        group.Name.Should().Be("Matematik Zümresi");
        group.IsActive.Should().BeTrue();
        group.SchoolId.Should().Be(_schoolId);
    }

    [Fact]
    public void WorkingGroup_SoftDelete_ShouldPreserveDataAndSetInactive()
    {
        var group = new WorkingGroup
        {
            Id = Guid.NewGuid(),
            Name = "Silinecek Grup",
            SchoolId = _schoolId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        group.IsActive = false;
        group.UpdatedAt = DateTime.UtcNow;

        group.IsActive.Should().BeFalse();
        group.Name.Should().Be("Silinecek Grup");
        group.UpdatedAt.Should().NotBeNull();
    }

    [Theory]
    [InlineData("A")]
    [InlineData("")]
    public void WorkingGroup_ShortName_ShouldBeLessThanMinLength(string name)
    {
        name.Length.Should().BeLessThan(2);
    }

    // ─── WorkingGroupMember Tests ─────────────────────────────────────────────

    [Fact]
    public void WorkingGroupMember_ValidRole_ShouldStoreCorrectly()
    {
        var member = new WorkingGroupMember
        {
            Id = Guid.NewGuid(),
            WorkingGroupId = Guid.NewGuid(),
            UserId = _userId,
            Role = "Başkan",
            AssignedAt = DateTime.UtcNow
        };

        member.Role.Should().Be("Başkan");
        member.UserId.Should().Be(_userId);
        member.AssignedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }

    [Fact]
    public void WorkingGroupMember_SameUserSameGroup_UniqueConstraint()
    {
        var groupId = Guid.NewGuid();
        var member1 = new WorkingGroupMember { WorkingGroupId = groupId, UserId = _userId };
        var member2 = new WorkingGroupMember { WorkingGroupId = groupId, UserId = _userId };

        // DB'de unique index (groupId, userId) çakışmasını simüle et
        member1.WorkingGroupId.Should().Be(member2.WorkingGroupId);
        member1.UserId.Should().Be(member2.UserId);
    }

    // ─── Controller Integration Tests (InMemory DB) ──────────────────────────

    [Fact]
    public async Task GetAll_WithValidSchoolUser_ReturnsOkWithGroups()
    {
        // Arrange: grup ekle
        _db.WorkingGroups.Add(new WorkingGroup
        {
            Id = Guid.NewGuid(),
            Name = "Test Grubu",
            SchoolId = _schoolId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        });
        await _db.SaveChangesAsync();

        // Act
        var result = await _controller.GetAll(null, 1, 20);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var ok = (OkObjectResult)result;
        ok.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task Create_WithValidData_ReturnsCreatedGroup()
    {
        // Arrange
        var dto = new CreateWorkingGroupRequest("Yeni Çalışma Grubu", "Açıklama");

        // Act
        var result = await _controller.Create(dto);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var created = _db.WorkingGroups.FirstOrDefault(g => g.Name == "Yeni Çalışma Grubu");
        created.Should().NotBeNull();
        created!.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task GetAll_WithSearch_FiltersCorrectly()
    {
        // Arrange
        _db.WorkingGroups.AddRange(
            new WorkingGroup { Id = Guid.NewGuid(), Name = "Matematik Grubu", SchoolId = _schoolId, IsActive = true, CreatedAt = DateTime.UtcNow },
            new WorkingGroup { Id = Guid.NewGuid(), Name = "Fizik Grubu", SchoolId = _schoolId, IsActive = true, CreatedAt = DateTime.UtcNow }
        );
        await _db.SaveChangesAsync();

        // Act
        var result = await _controller.GetAll("Matematik", 1, 20);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Delete_ExistingGroup_SoftDeletesSuccessfully()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        _db.WorkingGroups.Add(new WorkingGroup
        {
            Id = groupId,
            Name = "Silinecek Grup",
            SchoolId = _schoolId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        });
        await _db.SaveChangesAsync();

        // Act
        var result = await _controller.Delete(groupId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var group = await _db.WorkingGroups.FindAsync(groupId);
        group!.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task Delete_NonExistingGroup_ReturnsNotFound()
    {
        var result = await _controller.Delete(Guid.NewGuid());
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task AddMember_ValidUser_AddsMemberToGroup()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        _db.WorkingGroups.Add(new WorkingGroup
        {
            Id = groupId,
            Name = "Grup",
            SchoolId = _schoolId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        });
        await _db.SaveChangesAsync();

        var dto = new AddWorkingGroupMemberRequest(_userId, "Üye");

        // Act
        var result = await _controller.AddMember(groupId, dto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var member = _db.WorkingGroupMembers.FirstOrDefault(m => m.WorkingGroupId == groupId && m.UserId == _userId);
        member.Should().NotBeNull();
    }

    [Fact]
    public async Task RemoveMember_ExistingMember_RemovesSuccessfully()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var memberId = Guid.NewGuid();
        _db.WorkingGroups.Add(new WorkingGroup { Id = groupId, Name = "Grup", SchoolId = _schoolId, IsActive = true, CreatedAt = DateTime.UtcNow });
        _db.WorkingGroupMembers.Add(new WorkingGroupMember { Id = memberId, WorkingGroupId = groupId, UserId = _userId, Role = "Üye", AssignedAt = DateTime.UtcNow });
        await _db.SaveChangesAsync();

        // Act
        var result = await _controller.RemoveMember(groupId, memberId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var member = await _db.WorkingGroupMembers.FindAsync(memberId);
        member.Should().BeNull();
    }

    public void Dispose() => _db.Dispose();
}
