namespace TestResultsDashboard.Services.Models.Dto;

public class TestProjectDto
{
    public TestProjectDetailsDto TestProjectDetails { get; set; }
    public IEnumerable<TestResultDto> TestResults { get; set; }
    
    public TestResultDto LastRun => TestResults.OrderByDescending(x => x.CreatedAt).First();

    public TimeSpan AverageDurationLastFiveRuns
    {
        get
        {
            var successfulRuns = TestResults
                .Where(x => x.Duration > new TimeSpan())
                .OrderByDescending(x => x.CreatedAt)
                .Take(5);
        
            var average = TimeSpan.FromSeconds(successfulRuns.Select(s => s.Duration.TotalSeconds).Average());

            return average;
        }
    }
}