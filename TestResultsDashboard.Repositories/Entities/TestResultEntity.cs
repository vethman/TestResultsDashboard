using System;
using MongoDB.Entities;

namespace TestResultsDashboard.Repositories.Entities;

public class TestResultEntity : Entity
{
    public string TestProjectId { get; set; }
    public int Passed { get; set; }
    public int Failed { get; set; }
    public int Skipped { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}