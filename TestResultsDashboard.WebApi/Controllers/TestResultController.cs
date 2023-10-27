using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestResultsDashboard.Services;
using TestResultsDashboard.Services.Models;

namespace TestResultsDashboard.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TestResultController : ControllerBase
{
    private readonly ITestResultService _testResultService;

    public TestResultController(ITestResultService testResultService)
    {
        _testResultService = testResultService;
    }
    
    [HttpPost]
    public async Task<IActionResult> Insert([FromBody] TestResultCreateRequest createRequest)
    {
        await _testResultService.AddTestResultAsync(createRequest);
        
        return Ok();
    }
    
    [HttpGet("test-project-id/{testProjectId}/all")]
    public async Task<IActionResult> GetAll(string testProjectId)
    {
        var testResults = await _testResultService.GetAllTestResultsForTestProject(testProjectId);

        if (!testResults.Any())
            return NotFound($"No TestResults found for testProject with id: '{testProjectId}'");

        return Ok(testResults);
    }
}