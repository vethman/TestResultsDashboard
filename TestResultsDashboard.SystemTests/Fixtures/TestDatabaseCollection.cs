using TestResultsDashboard.TestDatabase;

namespace TestResultsDashboard.SystemTests.Fixtures
{
    [CollectionDefinition(nameof(TestDatabaseCollection))]
    public class TestDatabaseCollection : ICollectionFixture<TestDatabaseFixture>, IClassFixture<CustomWebApplicationFactory>
    {

    }

    public class TestDatabaseFixture : IAsyncLifetime
    {
        public ITestDatabase TestDatabase { get; private set; } = null!;

        public async Task InitializeAsync()
        {
            TestDatabase = new TestMongoDb();
            await TestDatabase.InitializeAsync();
            CustomWebApplicationFactory.SetTestDatabaseConfiguration(TestDatabase.Name, TestDatabase.Host, TestDatabase.Port);
        }

        public async Task DisposeAsync()
        {
            if (TestDatabase != null)
            {
                await TestDatabase.DisposeAsync();
            }
        }
    }
}
