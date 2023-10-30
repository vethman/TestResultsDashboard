using Microsoft.Extensions.DependencyInjection;
using TestResultsDashboard.Repositories.Entities;

namespace TestResultsDashboard.Repositories;

public static class DependencyInjectionConfiguration
{
    public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddTransient<IRepository<TestProjectDetailsEntity>, Repository<TestProjectDetailsEntity>>()
            .AddTransient<IRepository<TestResultEntity>, Repository<TestResultEntity>>()
            ;
    }
}