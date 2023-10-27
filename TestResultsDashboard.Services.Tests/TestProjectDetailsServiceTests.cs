using System;
using System.Threading.Tasks;
using Moq;
using TestResultsDashboard.Common.Enums;
using TestResultsDashboard.Repositories;
using TestResultsDashboard.Repositories.Entities;
using TestResultsDashboard.Services.Models;

namespace TestResultsDashboard.Services.Tests;

public class TestProjectDetailsServiceTests
{
    [Fact]
    public async Task AddTestProjectDetailsAsync_ShouldAddTestProjectDetailsAndReturnId()
    {
        var testProjectDetailsCreateRequest = new TestProjectDetailsCreateRequest
        {
            Name = "TestProject1",
            Version = 1,
            Summary = "Lorum ipsum bladiebla",
            TestCategory = TestCategory.Unit,
            CreatedBy = "Ronald"
        };

        var expectedId = Guid.NewGuid().ToString();
        
        var repositoryMock = new Mock<IRepository<TestProjectDetailsEntity>>();
                
        TestProjectDetailsEntity actualTestProjectDetailsEntity = null!;
        
        repositoryMock.Setup(x => x.InsertAsync(It.IsAny<TestProjectDetailsEntity>()))
            .ReturnsAsync(expectedId)
            .Callback<TestProjectDetailsEntity>(x => actualTestProjectDetailsEntity = x);

        var sut = new TestProjectDetailsService(repositoryMock.Object);
        var result = await sut.AddTestProjectDetailsAsync(testProjectDetailsCreateRequest);
        
        Assert.Equal(expectedId, result);
        
        Assert.Equal(testProjectDetailsCreateRequest.Name, actualTestProjectDetailsEntity.Name);
        Assert.Equal(testProjectDetailsCreateRequest.Version, actualTestProjectDetailsEntity.Version);
        Assert.Equal(testProjectDetailsCreateRequest.Summary, actualTestProjectDetailsEntity.Summary);
        Assert.Equal(testProjectDetailsCreateRequest.TestCategory, actualTestProjectDetailsEntity.TestCategory);
        Assert.Equal(testProjectDetailsCreateRequest.CreatedBy, actualTestProjectDetailsEntity.CreatedBy);
        Assert.Equal(testProjectDetailsCreateRequest.CreatedBy, actualTestProjectDetailsEntity.UpdatedBy);
        Assert.InRange(actualTestProjectDetailsEntity.CreatedAt, DateTime.UtcNow.AddMinutes(-10), DateTime.UtcNow.AddMinutes(10));
        Assert.Equal(actualTestProjectDetailsEntity.LastUpdatedAt, actualTestProjectDetailsEntity.CreatedAt);
    }
}