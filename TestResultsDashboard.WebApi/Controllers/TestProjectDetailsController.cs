using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestResultsDashboard.Services;
using TestResultsDashboard.Services.Models;
using TestResultsDashboard.Services.Models.Dto;

namespace TestResultsDashboard.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TestProjectDetailsController : ControllerBase
{
    private readonly ITestProjectDetailsService _testProjectDetailsService;

    public TestProjectDetailsController(
        ITestProjectDetailsService testProjectDetailsService)
    {
        _testProjectDetailsService = testProjectDetailsService;
    }

    [HttpPost]
    public async Task<IActionResult> Insert([FromBody] TestProjectDetailsCreateRequest createRequest)
    {
        var id = await _testProjectDetailsService.AddTestProjectDetailsAsync(createRequest);
        
        return Ok(id);
    }

    [HttpGet("all")]
    public async Task<IEnumerable<TestProjectDetailsDto>> GetAll()
    {
        return await _testProjectDetailsService.GetAllTestProjectDetailsAsync();
    }
    
    [HttpGet("totals")]
    public async Task<TestProjectDetailsTotalsDto> GetTotals()
    {
        return await _testProjectDetailsService.GetTotalsAsync();
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] TestProjectDetailsUpdateRequest updateRequest)
    {
        await _testProjectDetailsService.UpdateTestProjectDetailsAsync(updateRequest);

        return Ok();
    }
    
    [HttpPut("id/{id}/change-name")]
    public async Task<IActionResult> UpdateName(string id, [FromQuery] string name, string updatedBy)
    {
        if (!await _testProjectDetailsService.TestProjectDetailsIdExistsAsync(id))
            return BadRequest($"TestProjectDetails not found for ID: '{id}'");

        await _testProjectDetailsService.UpdateTestProjectDetailsNameAsync(id, name, updatedBy);

        return Ok();
    }
}