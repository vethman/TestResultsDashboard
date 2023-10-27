using System.Text.Json.Serialization;
using TestResultsDashboard.Common.Enums;
using TestResultsDashboard.Services.JsonConverters;

namespace TestResultsDashboard.Services.Models;

public class TestProjectDetailsUpdateRequest
{
    public string Name { get; set; }
    public int Version { get; set; }
    public string Summary { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumWithDefaultFallbackConverter))]
    public TestCategory TestCategory { get; set; }
    
    public string UpdatedBy { get; set; }
}