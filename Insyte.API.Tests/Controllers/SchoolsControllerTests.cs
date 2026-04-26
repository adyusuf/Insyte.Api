using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Insyte.Core.Entities;
using Insyte.Core.Enums;

namespace Insyte.API.Tests.Controllers;

/// <summary>
/// Okul entity ve domain kuralları için birim testler.
/// SchoolsController entegrasyon testleri için Testcontainers (PostgreSQL) gereklidir.
/// </summary>
public class SchoolEntityTests
{
    // ─── School Entity Tests ───────────────────────────────────────────────────

    [Fact]
    public void School_ValidEntity_ShouldHaveCorrectDefaults()
    {
        var school = new School
        {
            Id = Guid.NewGuid(),
            Name = "Anadolu Lisesi",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        school.Name.Should().Be("Anadolu Lisesi");
        school.IsActive.Should().BeTrue();
        school.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void School_SoftDelete_ShouldSetInactive()
    {
        var school = new School { Name = "Silinecek Okul", IsActive = true, CreatedAt = DateTime.UtcNow };
        school.IsActive = false;
        school.UpdatedAt = DateTime.UtcNow;

        school.IsActive.Should().BeFalse();
        school.Name.Should().Be("Silinecek Okul");
        school.UpdatedAt.Should().NotBeNull();
    }

    // ─── User Entity Tests ─────────────────────────────────────────────────────

    [Fact]
    public void User_ValidEntity_ShouldHaveCorrectProperties()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@insyte.com",
            PasswordHash = "hash",
            FirstName = "Test",
            LastName = "Kullanıcı",
            Role = UserRole.SchoolAdmin,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        user.Email.Should().Be("test@insyte.com");
        user.Role.Should().Be(UserRole.SchoolAdmin);
        user.IsActive.Should().BeTrue();
    }

    [Theory]
    [InlineData(UserRole.Admin)]
    [InlineData(UserRole.Advisor)]
    [InlineData(UserRole.SchoolAdmin)]
    [InlineData(UserRole.Teacher)]
    public void User_AllRoles_ShouldBeValidEnum(UserRole role)
    {
        Enum.IsDefined(typeof(UserRole), role).Should().BeTrue();
    }

    // ─── SchoolUser Entity Tests ───────────────────────────────────────────────

    [Fact]
    public void SchoolUser_Assignment_ShouldTrackTimestamp()
    {
        var schoolUser = new SchoolUser
        {
            Id = Guid.NewGuid(),
            SchoolId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Role = UserRole.Teacher,
            AssignedAt = DateTime.UtcNow
        };

        schoolUser.AssignedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
        schoolUser.Role.Should().Be(UserRole.Teacher);
    }

    // ─── InMemory DB Smoke Tests ───────────────────────────────────────────────

    [Fact]
    public async Task InMemoryDb_AddSchool_ShouldPersist()
    {
        using var db = CreateInMemoryDb();
        var school = new School { Id = Guid.NewGuid(), Name = "Test Okul", IsActive = true, CreatedAt = DateTime.UtcNow };
        db.Schools.Add(school);
        await db.SaveChangesAsync();

        var retrieved = await db.Schools.FindAsync(school.Id);
        retrieved.Should().NotBeNull();
        retrieved!.Name.Should().Be("Test Okul");
    }

    [Fact]
    public async Task InMemoryDb_AddUser_ShouldPersist()
    {
        using var db = CreateInMemoryDb();
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "u@test.com",
            PasswordHash = "hash",
            FirstName = "A",
            LastName = "B",
            Role = UserRole.Teacher,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();

        db.Users.Should().ContainSingle(u => u.Email == "u@test.com");
    }

    [Fact]
    public async Task InMemoryDb_SoftDeleteSchool_ShouldUpdateIsActive()
    {
        using var db = CreateInMemoryDb();
        var schoolId = Guid.NewGuid();
        db.Schools.Add(new School { Id = schoolId, Name = "Silinecek", IsActive = true, CreatedAt = DateTime.UtcNow });
        await db.SaveChangesAsync();

        var school = await db.Schools.FindAsync(schoolId);
        school!.IsActive = false;
        school.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();

        (await db.Schools.FindAsync(schoolId))!.IsActive.Should().BeFalse();
    }

    private static Insyte.Infrastructure.Data.InsyteDbContext CreateInMemoryDb()
    {
        var options = new DbContextOptionsBuilder<Insyte.Infrastructure.Data.InsyteDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new Insyte.Infrastructure.Data.InsyteDbContext(options);
    }
}
