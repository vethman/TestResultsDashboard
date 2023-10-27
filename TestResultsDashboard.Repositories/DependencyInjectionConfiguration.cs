using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Entities;
using TestResultsDashboard.Repositories.Configuration;
using TestResultsDashboard.Repositories.Entities;

namespace TestResultsDashboard.Repositories;

public static class DependencyInjectionConfiguration
{
    public static async Task<IServiceCollection> AddRepositoriesAsync(this IServiceCollection serviceCollection, IConfigurationRoot configurationRoot)
    {
        var databaseConfig = new DatabaseConfig();
        configurationRoot.GetSection("Database").Bind(databaseConfig); 
        
        await DB.InitAsync(databaseConfig.Name, databaseConfig.Host, databaseConfig.Port);

        return serviceCollection
            .AddTransient<IRepository<TestProjectDetailsEntity>, Repository<TestProjectDetailsEntity>>()
            .AddTransient<IRepository<TestResultEntity>, Repository<TestResultEntity>>()
            ;
    }
}