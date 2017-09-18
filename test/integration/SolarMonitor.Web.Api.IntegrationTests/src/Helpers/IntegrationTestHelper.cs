using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;
using Microsoft.Extensions.Configuration;

namespace SolarMonitor.Api.IntegrationTests
{
    [LogTestToConsole]
    //[Collection("Integration tests")]
    public class IntegrationTestHelper
    {
        protected readonly TestServer _server;
        protected readonly HttpClient _client;
        //protected readonly ApiServiceHelper _service;
        protected readonly DbHelper _db;

        //protected IntegrationTestFixture Fixture { get; private set; }
        protected IServiceProvider ServiceProvider { get; private set; }
        protected TokenProvider TokenProvider { get; private set; }

        // public IntegrationTestHelper(IntegrationTestFixture fixture = null)
        // {
        //     Fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        // }

        public IntegrationTestHelper()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<TestStartup>());

            ServiceProvider = _server.Host.Services;

            _client = _server.CreateClient();
            //_service = new ApiServiceHelper(_client);
            System.Type contextType = typeof(Data.Repositories.MySql.ApplicationDbContext);
            var context = _server.Host.Services.GetService(contextType);
            _db = new DbHelper(context as Data.Repositories.MySql.ApplicationDbContext);

            // set up the test configuration
            var envName = "Development";
            var settingsFile = $"appsettings.{envName}.json";
            var testConfiguration = new ConfigurationBuilder()
                .AddJsonFile(settingsFile, optional: true, reloadOnChange: false)
                //.AddUserSecrets("IdentityManagerSecrets")
                // use the secrets from the main SolarMonitor.Web.Api project
                .AddUserSecrets<SolarMonitorApi.Startup>()
                .Build();

            TokenProvider = new TokenProvider(testConfiguration);
        }
    }
}
