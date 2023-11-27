using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using TestResultsDashboard.Services;
using TestResultsDashboard.Services.Models;
using TestResultsDashboard.Services.Models.Dto;
using TestResultsDashboard.SystemTests.Fixtures;

namespace TestResultsDashboard.AppTests;

[Collection(nameof(TestDatabaseCollection))]
public class TestProjectControllerTests
{
    private readonly TestDatabaseFixture _fixture;
    private readonly CustomWebApplicationFactory _factory;

    public TestProjectControllerTests(
        TestDatabaseFixture fixture,
        CustomWebApplicationFactory factory)
    {
        _fixture = fixture;
        _factory = factory;
    }

    [Fact]
    public async Task Get_TestProjectExistsWithoutRuns_ReturnsTestprojectDto()
    {
        var client = _factory.CreateClient();
        var testProjectDetailsService = _factory.Services.GetRequiredService<ITestProjectDetailsService>();
        var testResultService = _factory.Services.GetRequiredService<ITestResultService>();

        var testProjectDetailsCreateRequest = new TestProjectDetailsCreateRequest
        {
            Name = "Name",
            Version = 1,
            Summary = "Goed verhaal lekker kort",
            TestCategory = Common.Enums.TestCategory.Unit,
            CreatedBy = "Ronaldoooo"
        };

        var testProjectId = await testProjectDetailsService.AddTestProjectDetailsAsync(testProjectDetailsCreateRequest);

        var testResultCreateRequest1 = new TestResultCreateRequest
        {
            TestProjectId = testProjectId,
            Passed = 1,
            Failed = 2,
            Skipped = 3,
            CreatedBy = "Ronaldoooootje1",
            Start = DateTime.Now.AddHours(-1),
            End = DateTime.Now.AddMinutes(-10)
        };

        var testResultCreateRequest2 = new TestResultCreateRequest
        {
            TestProjectId = testProjectId,
            Passed = 3,
            Failed = 2,
            Skipped = 1,
            CreatedBy = "Ronaldoooootje2",
            Start = DateTime.Now.AddMinutes(-30),
            End = DateTime.Now
        };

        await testResultService.AddTestResultAsync(testResultCreateRequest1);
        await testResultService.AddTestResultAsync(testResultCreateRequest2);

        var result = await client.GetAsync($"testproject/name={testProjectDetailsCreateRequest.Name}&version={testProjectDetailsCreateRequest.Version}");
        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var testProjectDtoJson = await result.Content.ReadAsStringAsync();
        var actualTestProjectDto = JsonConvert.DeserializeObject<TestProjectDto>(testProjectDtoJson);

        actualTestProjectDto!.TestResults.Should().HaveCount(2);
    }

    [Fact]
    public async Task Get_WhenNoTestProject_ShouldReturnNotFound()
    {
        var client = _factory.CreateClient();

        var name = "testname";
        var version = 0;
        var result = await client.GetAsync($"testproject/name={name}&version={version}");

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var message = await result.Content.ReadAsStringAsync();
        message.Should().Be($"No TestProject found for name: '{name}' and version: '{version}'");
    }
}