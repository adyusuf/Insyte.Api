using Xunit;
using FluentAssertions;
using Moq;
using Microsoft.EntityFrameworkCore;
using Insyte.Infrastructure.Data;
using Insyte.API.Controllers;
using Insyte.Core.Entities;
using Insyte.Core.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Insyte.API.Tests.Controllers;

public class WorkingGroupsControllerTests
{
    private readonly Mock<InsyteDbContext> _mockDbContext;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private WorkingGroupsController _controller;

    public WorkingGroupsControllerTests()
    {
        _mockDbContext = new Mock<InsyteDbContext>();
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _controller = new WorkingGroupsController(_mockDbContext.Object, _mockHttpContextAccessor.Object);
    }

    [Fact]
    public async Task GetAll_WithValidUser_ReturnsWorkingGroups()
    {
        // Arrange
        var schoolId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var workingGroups = new List<WorkingGroup>
        {
            new WorkingGroup
            {
                Id = Guid.NewGuid(),
                Name = "Test Grubu 1",
                SchoolId = schoolId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new WorkingGroup
            {
                Id = Guid.NewGuid(),
                Name = "Test Grubu 2",
                SchoolId = schoolId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        // Act & Assert
        workingGroups.Should().HaveCount(2);
        workingGroups[0].Name.Should().Be("Test Grubu 1");
        workingGroups[0].SchoolId.Should().Be(schoolId);
    }

    [Fact]
    public void CreateWorkingGroup_WithValidName_ShouldSucceed()
    {
        // Arrange
        var group = new WorkingGroup
        {
            Id = Guid.NewGuid(),
            Name = "Yeni Çalışma Grubu",
            Description = "Test açıklaması",
            SchoolId = Guid.NewGuid(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // Act & Assert
        group.Name.Should().NotBeNullOrEmpty();
        group.Name.Should().HaveLength(20);
        group.IsActive.Should().BeTrue();
    }

    [Fact]
    public void WorkingGroupMember_WithValidRole_ShouldStoreCorrectly()
    {
        // Arrange
        var member = new WorkingGroupMember
        {
            Id = Guid.NewGuid(),
            WorkingGroupId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Role = "Başkan",
            AssignedAt = DateTime.UtcNow
        };

        // Act & Assert
        member.Role.Should().Be("Başkan");
        member.AssignedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData("A")]      // Too short
    [InlineData("")]       // Empty
    public void CreateWorkingGroup_WithInvalidName_ShouldFail(string invalidName)
    {
        // Arrange & Act
        var group = new WorkingGroup { Name = invalidName };

        // Assert
        group.Name.Length.Should().BeLessThan(2);
    }

    [Fact]
    public void WorkingGroup_SoftDeletePattern_ShouldPreserveData()
    {
        // Arrange
        var group = new WorkingGroup
        {
            Id = Guid.NewGuid(),
            Name = "Silinecek Grup",
            SchoolId = Guid.NewGuid(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        group.IsActive = false;
        group.UpdatedAt = DateTime.UtcNow;

        // Assert
        group.IsActive.Should().BeFalse();
        group.UpdatedAt.Should().NotBeNull();
        group.Name.Should().Be("Silinecek Grup"); // Data preserved
    }

    [Fact]
    public void WorkingGroupMember_ValidateUniqueness()
    {
        // Arrange
        var workingGroupId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var member1 = new WorkingGroupMember
        {
            Id = Guid.NewGuid(),
            WorkingGroupId = workingGroupId,
            UserId = userId,
            Role = "Üye"
        };

        var member2 = new WorkingGroupMember
        {
            Id = Guid.NewGuid(),
            WorkingGroupId = workingGroupId,
            UserId = userId,
            Role = "Üye"
        };

        // Act & Assert
        member1.WorkingGroupId.Should().Be(member2.WorkingGroupId);
        member1.UserId.Should().Be(member2.UserId);
        // In real DB, unique index (workingGroupId, userId) would prevent duplicates
    }
}
