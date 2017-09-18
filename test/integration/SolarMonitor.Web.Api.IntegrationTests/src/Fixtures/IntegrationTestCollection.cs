using Xunit;

namespace SolarMonitor.Api.IntegrationTests
{
    [CollectionDefinition("Integration tests")]
    public class IntegrationTestCollection : ICollectionFixture<IntegrationTestFixture>
    {
        // Intentionally left blank.
        // This class only serves as an anchor for CollectionDefinition.
    }
}
