using FluentAssertions;
using System.Net;
using TestResultsDashboard.SystemTests.Fixtures;

namespace TestResultsDashboard.SystemTests;

[Collection(nameof(TestDatabaseCollection))]
public class TestProjectControllerTests
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly TestDatabaseFixture _fixture;

    public TestProjectControllerTests(
        TestDatabaseFixture fixture,
        CustomWebApplicationFactory factory)
    {
        _fixture = fixture;
        _factory = factory;
        
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