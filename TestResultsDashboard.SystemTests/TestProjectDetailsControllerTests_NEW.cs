using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using TestResultsDashboard.AppTests.Builders;
using TestResultsDashboard.AppTests.Fixtures;
using TestResultsDashboard.Services.Models.Dto;

namespace TestResultsDashboard.AppTests;

[Collection(nameof(TestDatabaseCollection))]
public class TestProjectDetailsControllerTests_NEW
{
    private readonly TestDatabaseFixture _fixture;
    private readonly CustomWebApplicationFactory _factory;

    public TestProjectDetailsControllerTests_NEW(
    TestDatabaseFixture fixture,
    CustomWebApplicationFactory factory)
    {
        _fixture = fixture;
        _factory = factory;
    }

    [Fact]
    public async Task TestProjectDetailsCreateRequest_PostReturnsTestProjectId_GetReturnsInsertedTestProjectDetails()
    {
        var client = _factory.CreateClient();

        var testProjectDetailsCreateRequest = TestProjectDetailsCreateRequestBuilder
            .New()
            .WithDefault()
            .Build();

        var postResult = await client.PostAsJsonAsync("testprojectdetails", testProjectDetailsCreateRequest);
        postResult.StatusCode.Should().Be(HttpStatusCode.OK);

        var testProjectId = await postResult.Content.ReadAsStringAsync();

        var expectedTestProjectDetailsDto = new TestProjectDetailsDto
        {
            Id = testProjectId,
            Name = testProjectDetailsCreateRequest.Name,
            Version = testProjectDetailsCreateRequest.Version,
            Summary = testProjectDetailsCreateRequest.Summary,
            TestCategory = testProjectDetailsCreateRequest.TestCategory,
            UpdatedBy = testProjectDetailsCreateRequest.CreatedBy,
            LastUpdatedAt = DateTime.UtcNow
        };

        var getResult = await client.GetFromJsonAsync<List<TestProjectDetailsDto>>("testprojectdetails/all");

        var actualTestProjectDetailsDto = getResult!.Single(x => x.Id == testProjectId);

        actualTestProjectDetailsDto.Should().BeEquivalentTo(expectedTestProjectDetailsDto, options => options
            .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, new TimeSpan(0, 30, 0)))
            .WhenTypeIs<DateTime>());
    }
}
