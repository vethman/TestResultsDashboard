using Microsoft.Extensions.DependencyInjection;

namespace TestResultsDashboard.TestDatabase;

public static class DependencyInjectionConfiguration
{
    public static IServiceCollection AddTestDatabase(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddSingleton<ITestDatabase, TestMongoDb>()
            ;
    }
}