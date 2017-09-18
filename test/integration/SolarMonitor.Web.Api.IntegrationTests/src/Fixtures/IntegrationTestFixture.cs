using System;
using SolarMonitor.Data.Adapters;
using Xunit;

namespace SolarMonitor.Api.IntegrationTests
{
    public class IntegrationTestFixture : IDisposable
    {
        public ISiteAdapter SiteAdapter { get; private set; }
        public IntegrationTestFixture() // ISiteAdapter siteAdapter)
        {
            // (setup code)
            //SiteAdapter = siteAdapter ?? throw new ArgumentNullException(nameof(siteAdapter));
        }

        public void Dispose()
        {
            // (teardown code)
        }

        public string GeneratedTestName { get; private set; }
    }
}
