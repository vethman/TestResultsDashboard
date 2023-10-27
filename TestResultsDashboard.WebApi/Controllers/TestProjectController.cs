using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestResultsDashboard.Services;

namespace TestResultsDashboard.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TestProjectController : ControllerBase
{
    private readonly ITestProjectService _testProjectService;

    public TestProjectController(ITestProjectService testProjectService)
    {
        _testProjectService = testProjectService;
    }
    
    [HttpGet("name={name}&version={version:int}")]
    public async Task<IActionResult> Get(string name, int version)
    {
        var testProject = await _testProjectService.GetTestProjectAsync(name, version);

        if (testProject == null)
            return NotFound($"No TestProject found for name: '{name}' and version: '{version}'");

        return Ok(testProject);
    }
}