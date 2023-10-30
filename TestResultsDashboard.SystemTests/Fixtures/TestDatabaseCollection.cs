using TestResultsDashboard.TestDatabase;

namespace TestResultsDashboard.SystemTests.Fixtures
{
    [Collection(nameof(TestDatabaseCollection))]
    public class TestDatabaseCollection : ICollectionFixture<TestDatabaseFixture>
    {

    }

    public class TestDatabaseFixture : IAsyncLifetime
    {
        public ITestDatabase TestDatabase { get; private set; } = null!;

        public async Task InitializeAsync()
        {
            TestDatabase = new TestMongoDb();
            await TestDatabase.InitializeAsync();
        }

        public async Task DisposeAsync()
        {
            await TestDatabase.DisposeAsync();
        }
    }
}
