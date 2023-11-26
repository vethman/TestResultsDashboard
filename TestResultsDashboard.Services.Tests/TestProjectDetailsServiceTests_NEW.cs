using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using TestResultsDashboard.Repositories;
using TestResultsDashboard.Repositories.Entities;
using TestResultsDashboard.Services.Models;
using TestResultsDashboard.Services.Tests.AutoFixture;

namespace TestResultsDashboard.Services.Tests;

public class TestProjectDetailsServiceTests_NEW
{
    [AutoDomainInlineData, Theory]
    public async Task AddTestProjectDetailsAsync_ShouldAddTestProjectDetailsAndReturnId(
        [Frozen] Mock<IRepository<TestProjectDetailsEntity>> repositoryMock,
        TestProjectDetailsCreateRequest testProjectDetailsCreateRequest,
        string expectedId,
        TestProjectDetailsService sut)
    {
        TestProjectDetailsEntity actualTestProjectDetailsEntity = null!;

        repositoryMock.Setup(x => x.InsertAsync(It.IsAny<TestProjectDetailsEntity>()))
            .ReturnsAsync(expectedId)
            .Callback<TestProjectDetailsEntity>(x => actualTestProjectDetailsEntity = x);

        var result = await sut.AddTestProjectDetailsAsync(testProjectDetailsCreateRequest);

        result.Should().Be(expectedId);

        var expectedTestProjectDetailsEntity = new TestProjectDetailsEntity
        {
            Name = testProjectDetailsCreateRequest.Name,
            Version = testProjectDetailsCreateRequest.Version,
            Summary = testProjectDetailsCreateRequest.Summary,
            TestCategory = testProjectDetailsCreateRequest.TestCategory,
            CreatedBy = testProjectDetailsCreateRequest.CreatedBy,
            CreatedAt = DateTime.UtcNow,
            UpdatedBy = testProjectDetailsCreateRequest.CreatedBy,
            LastUpdatedAt = DateTime.UtcNow
        };

        actualTestProjectDetailsEntity.Should().BeEquivalentTo(expectedTestProjectDetailsEntity, options => options
            .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, new TimeSpan(0, 30, 0)))
            .WhenTypeIs<DateTime>());
    }
}