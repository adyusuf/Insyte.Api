using Xunit;
using FluentAssertions;
using Insyte.Core.Entities;
using System;
using System.Collections.Generic;

namespace Insyte.API.Tests.Controllers;

public class CouncilsControllerTests
{
    [Fact]
    public void CreateCouncil_WithValidData_ShouldInitializeCorrectly()
    {
        // Arrange
        var council = new Council
        {
            Id = Guid.NewGuid(),
            Name = "Yönetim Kurulu",
            Description = "Okul yönetim kurulu",
            SchoolId = Guid.NewGuid(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // Act & Assert
        council.Name.Should().Be("Yönetim Kurulu");
        council.IsActive.Should().BeTrue();
        council.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void CouncilMember_WithValidRole_ShouldStoreCorrectly()
    {
        // Arrange
        var member = new CouncilMember
        {
            Id = Guid.NewGuid(),
            CouncilId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Role = "Başkan Yardımcısı",
            AssignedAt = DateTime.UtcNow
        };

        // Act & Assert
        member.Role.Should().Be("Başkan Yardımcısı");
        member.AssignedAt.Should().NotBeDefault();
    }

    [Theory]
    [InlineData("Okul Meclisi")]
    [InlineData("Disiplin Kurulu")]
    [InlineData("Rehberlik Kurulu")]
    public void CreateCouncil_WithValidNames_ShouldSucceed(string name)
    {
        // Arrange & Act
        var council = new Council { Name = name };

        // Assert
        council.Name.Should().Be(name);
        council.Name.Length.Should().BeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public void CouncilMember_ValidateMultipleMembersInSameCouncil()
    {
        // Arrange
        var councilId = Guid.NewGuid();
        var members = new List<CouncilMember>
        {
            new CouncilMember { Id = Guid.NewGuid(), CouncilId = councilId, UserId = Guid.NewGuid(), Role = "Başkan" },
            new CouncilMember { Id = Guid.NewGuid(), CouncilId = councilId, UserId = Guid.NewGuid(), Role = "Üye" },
            new CouncilMember { Id = Guid.NewGuid(), CouncilId = councilId, UserId = Guid.NewGuid(), Role = "Üye" }
        };

        // Act & Assert
        members.Should().HaveCount(3);
        members.ForEach(m => m.CouncilId.Should().Be(councilId));
    }

    [Fact]
    public void Council_SoftDelete_PreservesData()
    {
        // Arrange
        var council = new Council
        {
            Id = Guid.NewGuid(),
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
        council.SchoolId.Should().NotBeEmpty();
    }

    [Fact]
    public void CouncilMember_CheckTimestampAccuracy()
    {
        // Arrange
        var beforeCreation = DateTime.UtcNow;
        var member = new CouncilMember
        {
            Id = Guid.NewGuid(),
            CouncilId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Role = "Üye",
            AssignedAt = DateTime.UtcNow
        };
        var afterCreation = DateTime.UtcNow;

        // Act & Assert
        member.AssignedAt.Should().BeOnOrAfter(beforeCreation);
        member.AssignedAt.Should().BeOnOrBefore(afterCreation);
    }
}
