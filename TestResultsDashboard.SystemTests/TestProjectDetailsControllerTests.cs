using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using TestResultsDashboard.AppTests.Fixtures;
using TestResultsDashboard.Services.Models;
using TestResultsDashboard.Services.Models.Dto;

namespace TestResultsDashboard.AppTests;

[Collection(nameof(TestDatabaseCollection))]
public class TestProjectDetailsControllerTests
{
    private readonly TestDatabaseFixture _fixture;
    private readonly CustomWebApplicationFactory _factory;

    public TestProjectDetailsControllerTests(
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

        var testProjectDetailsCreateRequest = new TestProjectDetailsCreateRequest
        {
            Name = "Dit is een naam",
            Version = 1,
            Summary = "Goed verhaal lekker kort",
            TestCategory = Common.Enums.TestCategory.Integration,
            CreatedBy = "Ronaldoooo"
        };

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

        getResult.Should().Contain(expectedTestProjectDetailsDto);
    }
}
