using ExampleRestAPI.Test.APITests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace ExampleRestAPI.Test
{
    public class TestExampleRestAPIFactory<TStartup> : WebApplicationFactory<TStartup> 
        where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.Sources.Clear();
                config.SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("TestSettings.json")
                      .AddUserSecrets<APITestBase>();
            });
        }
    }
}
