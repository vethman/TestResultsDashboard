namespace TestResultsDashboard.TestDatabase;

public interface ITestDatabase
{
    string Name { get; set; }
    string Host { get; set; }
    int Port { get; set; }
    Task InitializeAsync();
    Task DisposeAsync();
}