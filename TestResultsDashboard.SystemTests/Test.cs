using TestResultsDashboard.TestDatabase;

namespace TestResultsDashboard.SystemTests;

public class Test
{
    [Fact]
    public async Task Test1()
    {
        var database = new TestMongoDb();

        await database.InitializeAsync();
        await database.DisposeAsync();
    }
}