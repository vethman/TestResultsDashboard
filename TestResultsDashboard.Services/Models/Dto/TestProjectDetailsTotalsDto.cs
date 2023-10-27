namespace TestResultsDashboard.Services.Models.Dto;

public class TestProjectDetailsTotalsDto
{
    public int Unit { get; set; }
    public int Integration { get; set; }
    public int UserInterface { get; set; }
    public string UnitPercentage { get; set; }
    public string IntegrationPercentage { get; set; }
    public string UserInterfacePercentage { get; set; }
    public int TotalTestProjects { get; set; }
}