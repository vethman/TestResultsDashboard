using System.Threading.Tasks;
using TestResultsDashboard.Services.Models.Dto;

namespace TestResultsDashboard.Services;

public interface ITestProjectService
{
    Task<TestProjectDto?> GetTestProjectAsync(string name, int version);
}

public class TestProjectService : ITestProjectService
{
    private readonly ITestProjectDetailsService _testProjectDetailsService;
    private readonly ITestResultService _testResultService;

    public TestProjectService(
        ITestProjectDetailsService testProjectDetailsService,
        ITestResultService testResultService)
    {
        _testProjectDetailsService = testProjectDetailsService;
        _testResultService = testResultService;
    }

    public async Task<TestProjectDto?> GetTestProjectAsync(string name, int version)
    {
        var testProjectDetails = await _testProjectDetailsService.GetTestProjectDetailsAsync(name, version);

        if (testProjectDetails == null)
            return null;

        var testResults = await _testResultService.GetAllTestResultsForTestProject(testProjectDetails.Id);

        return new TestProjectDto
        {
            TestProjectDetails = testProjectDetails,
            TestResults = testResults
        };
    }
}