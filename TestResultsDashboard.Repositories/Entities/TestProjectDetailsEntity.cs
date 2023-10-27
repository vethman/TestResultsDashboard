using MongoDB.Entities;
using TestResultsDashboard.Common.Enums;

namespace TestResultsDashboard.Repositories.Entities;

public class TestProjectDetailsEntity : Entity
{
    public string Name { get; set; }
    public int Version { get; set; }
    public string Summary { get; set; }
    public TestCategory TestCategory { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime LastUpdatedAt { get; set; }
}