using System;
using TestResultsDashboard.Common.Enums;

namespace TestResultsDashboard.Services.Models.Dto;

public class TestResultDto
{
    public int PassedTests { get; set; }
    public int FailedTests { get; set; }
    public int SkippedTests { get; set; }
    public int TotalTests { get; set; }
    public Status Status { get; set; }
    public string PassedPercentage { get; set; }
    public string FailedPercentage { get; set; }
    public string SkippedPercentage { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public TimeSpan Duration { get; set; }
}