using Testcontainers.MongoDb;

namespace TestResultsDashboard.TestDatabase;

public class TestMongoDb : ITestDatabase
{
    public string Name { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    
    private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder()
        .WithName($"TestResultsDashboard_TestContainer_{Guid.NewGuid().ToString()}")
        .WithPortBinding(27017, true)
        .Build();

    public async Task InitializeAsync()
    {
        await _mongoDbContainer.StartAsync();

        Name = _mongoDbContainer.Name;
        Host = _mongoDbContainer.Hostname;
        Port = _mongoDbContainer.GetMappedPublicPort(27017);
    }

    public async Task DisposeAsync()
    { 
        await _mongoDbContainer.DisposeAsync().AsTask();
    }
}