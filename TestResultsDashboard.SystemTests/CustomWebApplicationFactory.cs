using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace TestResultsDashboard.SystemTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private static string _testDatabaseName;
        private static string _testDatabaseHost;
        private static int _testDatabasePort;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Development");
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureHostConfiguration(config =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string> 
                { 
                    { "Database:Name", _testDatabaseName },
                    { "Database:Host", _testDatabaseHost },
                    { "Database:Port", _testDatabasePort.ToString() }
                }!);
            });

            return base.CreateHost(builder);
        }

        public static void SetTestDatabaseConfiguration(string name, string host, int port)
        {
            _testDatabaseName = name;
            _testDatabaseHost = host;
            _testDatabasePort = port;
        }
    }
}