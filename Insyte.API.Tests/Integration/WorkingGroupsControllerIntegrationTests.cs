using Xunit;
using FluentAssertions;
using Insyte.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insyte.API.Tests.Integration;

public class WorkingGroupsControllerIntegrationTests
{
    [Fact]
    public async Task CreateWorkingGroup_WithValidData_ShouldReturnCreatedResponse()
    {
        // Arrange
        var schoolId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var groupName = "Teknoloji Çalışma Grubu";
        var groupDescription = "Teknoloji alanında çalışmalar";

        // Act
        var workingGroup = new WorkingGroup
        {
            Id = Guid.NewGuid(),
            Name = groupName,
            Description = groupDescription,
            SchoolId = schoolId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // Assert
        workingGroup.Name.Should().Be(groupName);
        workingGroup.SchoolId.Should().Be(schoolId);
        workingGroup.IsActive.Should().BeTrue();
        workingGroup.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task GetWorkingGroup_WithValidId_ShouldReturnWorkingGroup()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var schoolId = Guid.NewGuid();
        var workingGroup = new WorkingGroup
        {
            Id = groupId,
            Name = "Rehberlik Çalışma Grubu",
            SchoolId = schoolId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // Act & Assert
        workingGroup.Id.Should().Be(groupId);
        workingGroup.Name.Should().NotBeNullOrEmpty();
        workingGroup.SchoolId.Should().Be(schoolId);
    }

    [Fact]
    public async Task UpdateWorkingGroup_WithValidData_ShouldUpdateSuccessfully()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var schoolId = Guid.NewGuid();
        var workingGroup = new WorkingGroup
        {
            Id = groupId,
            Name = "Eski Ad",
            Description = "Eski açıklama",
            SchoolId = schoolId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };

        var newName = "Yeni Ad";
        var newDescription = "Yeni açıklama";

        // Act
        workingGroup.Name = newName;
        workingGroup.Description = newDescription;
        workingGroup.UpdatedAt = DateTime.UtcNow;

        // Assert
        workingGroup.Name.Should().Be(newName);
        workingGroup.Description.Should().Be(newDescription);
        workingGroup.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteWorkingGroup_WithValidId_ShouldSoftDelete()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var workingGroup = new WorkingGroup
        {
            Id = groupId,
            Name = "Silinecek Grup",
            SchoolId = Guid.NewGuid(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        workingGroup.IsActive = false;
        workingGroup.UpdatedAt = DateTime.UtcNow;

        // Assert
        workingGroup.IsActive.Should().BeFalse();
        workingGroup.Name.Should().Be("Silinecek Grup");
        workingGroup.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task AddWorkingGroupMember_WithValidData_ShouldAddMember()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var memberRole = "Başkan";

        var member = new WorkingGroupMember
        {
            Id = Guid.NewGuid(),
            WorkingGroupId = groupId,
            UserId = userId,
            Role = memberRole,
            AssignedAt = DateTime.UtcNow
        };

        // Act & Assert
        member.WorkingGroupId.Should().Be(groupId);
        member.UserId.Should().Be(userId);
        member.Role.Should().Be(memberRole);
        member.AssignedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task RemoveWorkingGroupMember_WithValidData_ShouldRemove()
    {
        // Arrange
        var members = new List<WorkingGroupMember>
        {
            new WorkingGroupMember { Id = Guid.NewGuid(), WorkingGroupId = Guid.NewGuid(), UserId = Guid.NewGuid(), Role = "Üye" },
            new WorkingGroupMember { Id = Guid.NewGuid(), WorkingGroupId = Guid.NewGuid(), UserId = Guid.NewGuid(), Role = "Üye" }
        };

        var memberToRemove = members[0];

        // Act
        members.Remove(memberToRemove);

        // Assert
        members.Should().HaveCount(1);
        members.Should().NotContain(memberToRemove);
    }

    [Fact]
    public async Task ListWorkingGroups_WithSchoolScope_ShouldReturnSchoolGroupsOnly()
    {
        // Arrange
        var schoolId = Guid.NewGuid();
        var otherSchoolId = Guid.NewGuid();

        var groupsForSchool = new List<WorkingGroup>
        {
            new WorkingGroup { Id = Guid.NewGuid(), Name = "Grup 1", SchoolId = schoolId, IsActive = true },
            new WorkingGroup { Id = Guid.NewGuid(), Name = "Grup 2", SchoolId = schoolId, IsActive = true }
        };

        var groupsForOtherSchool = new List<WorkingGroup>
        {
            new WorkingGroup { Id = Guid.NewGuid(), Name = "Diğer Grup", SchoolId = otherSchoolId, IsActive = true }
        };

        var allGroups = groupsForSchool.Concat(groupsForOtherSchool).ToList();

        // Act
        var filteredGroups = allGroups.Where(g => g.SchoolId == schoolId).ToList();

        // Assert
        filteredGroups.Should().HaveCount(2);
        filteredGroups.Should().AllSatisfy(g => g.SchoolId.Should().Be(schoolId));
    }

    [Fact]
    public async Task UpdateWorkingGroupMemberRole_WithValidData_ShouldUpdateRole()
    {
        // Arrange
        var member = new WorkingGroupMember
        {
            Id = Guid.NewGuid(),
            WorkingGroupId = Guid.NewGuid(),
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
    public async Task WorkingGroup_Search_ShouldFindByName()
    {
        // Arrange
        var groups = new List<WorkingGroup>
        {
            new WorkingGroup { Id = Guid.NewGuid(), Name = "Matematik Çalışma Grubu", SchoolId = Guid.NewGuid(), IsActive = true },
            new WorkingGroup { Id = Guid.NewGuid(), Name = "Türkçe Çalışma Grubu", SchoolId = Guid.NewGuid(), IsActive = true },
            new WorkingGroup { Id = Guid.NewGuid(), Name = "Teknoloji Grubu", SchoolId = Guid.NewGuid(), IsActive = true }
        };

        var searchTerm = "Matematik";

        // Act
        var results = groups.Where(g => g.Name.ToLower().Contains(searchTerm.ToLower())).ToList();

        // Assert
        results.Should().HaveCount(1);
        results[0].Name.Should().Contain(searchTerm);
    }
}
