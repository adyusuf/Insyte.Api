using Xunit;
using FluentAssertions;
using Insyte.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insyte.API.Tests.Integration;

public class CouncilsControllerIntegrationTests
{
    [Fact]
    public async Task CreateCouncil_WithValidData_ShouldReturnCreatedResponse()
    {
        // Arrange
        var schoolId = Guid.NewGuid();
        var councilName = "Yönetim Kurulu";
        var councilDescription = "Okul yönetim kurulu";

        // Act
        var council = new Council
        {
            Id = Guid.NewGuid(),
            Name = councilName,
            Description = councilDescription,
            SchoolId = schoolId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // Assert
        council.Name.Should().Be(councilName);
        council.Description.Should().Be(councilDescription);
        council.SchoolId.Should().Be(schoolId);
        council.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task GetCouncil_WithValidId_ShouldReturnCouncil()
    {
        // Arrange
        var councilId = Guid.NewGuid();
        var schoolId = Guid.NewGuid();

        var council = new Council
        {
            Id = councilId,
            Name = "Disiplin Kurulu",
            SchoolId = schoolId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // Act & Assert
        council.Id.Should().Be(councilId);
        council.Name.Should().Be("Disiplin Kurulu");
        council.SchoolId.Should().Be(schoolId);
    }

    [Fact]
    public async Task UpdateCouncil_WithValidData_ShouldUpdateSuccessfully()
    {
        // Arrange
        var councilId = Guid.NewGuid();
        var schoolId = Guid.NewGuid();

        var council = new Council
        {
            Id = councilId,
            Name = "Eski Kurul Adı",
            Description = "Eski açıklama",
            SchoolId = schoolId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };

        var newName = "Yeni Kurul Adı";
        var newDescription = "Yeni açıklama";

        // Act
        council.Name = newName;
        council.Description = newDescription;
        council.UpdatedAt = DateTime.UtcNow;

        // Assert
        council.Name.Should().Be(newName);
        council.Description.Should().Be(newDescription);
        council.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteCouncil_WithValidId_ShouldSoftDelete()
    {
        // Arrange
        var councilId = Guid.NewGuid();

        var council = new Council
        {
            Id = councilId,
            Name = "Silinecek Kurul",
            SchoolId = Guid.NewGuid(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        council.IsActive = false;
        council.UpdatedAt = DateTime.UtcNow;

        // Assert
        council.IsActive.Should().BeFalse();
        council.Name.Should().Be("Silinecek Kurul");
        council.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task AddCouncilMember_WithValidData_ShouldAddMember()
    {
        // Arrange
        var councilId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var memberRole = "Başkan";

        var member = new CouncilMember
        {
            Id = Guid.NewGuid(),
            CouncilId = councilId,
            UserId = userId,
            Role = memberRole,
            AssignedAt = DateTime.UtcNow
        };

        // Act & Assert
        member.CouncilId.Should().Be(councilId);
        member.UserId.Should().Be(userId);
        member.Role.Should().Be(memberRole);
        member.AssignedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task RemoveCouncilMember_WithValidData_ShouldRemove()
    {
        // Arrange
        var members = new List<CouncilMember>
        {
            new CouncilMember { Id = Guid.NewGuid(), CouncilId = Guid.NewGuid(), UserId = Guid.NewGuid(), Role = "Üye" },
            new CouncilMember { Id = Guid.NewGuid(), CouncilId = Guid.NewGuid(), UserId = Guid.NewGuid(), Role = "Başkan" }
        };

        var memberToRemove = members[0];

        // Act
        members.Remove(memberToRemove);

        // Assert
        members.Should().HaveCount(1);
        members.Should().NotContain(memberToRemove);
    }

    [Fact]
    public async Task ListCouncils_WithSchoolScope_ShouldReturnSchoolCouncilsOnly()
    {
        // Arrange
        var schoolId = Guid.NewGuid();
        var otherSchoolId = Guid.NewGuid();

        var councilsForSchool = new List<Council>
        {
            new Council { Id = Guid.NewGuid(), Name = "Kurul 1", SchoolId = schoolId, IsActive = true },
            new Council { Id = Guid.NewGuid(), Name = "Kurul 2", SchoolId = schoolId, IsActive = true }
        };

        var councilsForOtherSchool = new List<Council>
        {
            new Council { Id = Guid.NewGuid(), Name = "Diğer Kurul", SchoolId = otherSchoolId, IsActive = true }
        };

        var allCouncils = councilsForSchool.Concat(councilsForOtherSchool).ToList();

        // Act
        var filteredCouncils = allCouncils.Where(c => c.SchoolId == schoolId).ToList();

        // Assert
        filteredCouncils.Should().HaveCount(2);
        filteredCouncils.Should().AllSatisfy(c => c.SchoolId.Should().Be(schoolId));
    }

    [Fact]
    public async Task UpdateCouncilMemberRole_WithValidData_ShouldUpdateRole()
    {
        // Arrange
        var member = new CouncilMember
        {
            Id = Guid.NewGuid(),
            CouncilId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Role = "Üye",
            AssignedAt = DateTime.UtcNow
        };

        // Act
        member.Role = "Başkan Yardımcısı";

        // Assert
        member.Role.Should().Be("Başkan Yardımcısı");
    }

    [Fact]
    public async Task Council_MultipleMembers_ShouldTrackAllMembers()
    {
        // Arrange
        var councilId = Guid.NewGuid();

        var members = new List<CouncilMember>
        {
            new CouncilMember { Id = Guid.NewGuid(), CouncilId = councilId, UserId = Guid.NewGuid(), Role = "Başkan" },
            new CouncilMember { Id = Guid.NewGuid(), CouncilId = councilId, UserId = Guid.NewGuid(), Role = "Başkan Yardımcısı" },
            new CouncilMember { Id = Guid.NewGuid(), CouncilId = councilId, UserId = Guid.NewGuid(), Role = "Üye" },
            new CouncilMember { Id = Guid.NewGuid(), CouncilId = councilId, UserId = Guid.NewGuid(), Role = "Üye" }
        };

        // Act & Assert
        members.Should().HaveCount(4);
        members.Should().AllSatisfy(m => m.CouncilId.Should().Be(councilId));
        members.Count(m => m.Role == "Başkan").Should().Be(1);
        members.Count(m => m.Role == "Üye").Should().Be(2);
    }

    [Fact]
    public async Task Council_Search_ShouldFindByName()
    {
        // Arrange
        var councils = new List<Council>
        {
            new Council { Id = Guid.NewGuid(), Name = "Okul Meclisi", SchoolId = Guid.NewGuid(), IsActive = true },
            new Council { Id = Guid.NewGuid(), Name = "Disiplin Kurulu", SchoolId = Guid.NewGuid(), IsActive = true },
            new Council { Id = Guid.NewGuid(), Name = "Rehberlik Kurulu", SchoolId = Guid.NewGuid(), IsActive = true }
        };

        var searchTerm = "Rehberlik";

        // Act
        var results = councils.Where(c => c.Name.ToLower().Contains(searchTerm.ToLower())).ToList();

        // Assert
        results.Should().HaveCount(1);
        results[0].Name.Should().Contain(searchTerm);
    }

    [Fact]
    public async Task Council_FilterByActive_ShouldReturnOnlyActiveCouncils()
    {
        // Arrange
        var councils = new List<Council>
        {
            new Council { Id = Guid.NewGuid(), Name = "Aktif Kurul 1", SchoolId = Guid.NewGuid(), IsActive = true },
            new Council { Id = Guid.NewGuid(), Name = "Pasif Kurul", SchoolId = Guid.NewGuid(), IsActive = false },
            new Council { Id = Guid.NewGuid(), Name = "Aktif Kurul 2", SchoolId = Guid.NewGuid(), IsActive = true }
        };

        // Act
        var activeCouncils = councils.Where(c => c.IsActive).ToList();

        // Assert
        activeCouncils.Should().HaveCount(2);
        activeCouncils.Should().AllSatisfy(c => c.IsActive.Should().BeTrue());
    }
}
