using System;

namespace TestResultsDashboard.Services.Models;

public class TestResultCreateRequest
{
    public string TestProjectId { get; set; }
    public int Passed { get; set; }
    public int Failed { get; set; }
    public int Skipped { get; set; }
    public string CreatedBy { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}