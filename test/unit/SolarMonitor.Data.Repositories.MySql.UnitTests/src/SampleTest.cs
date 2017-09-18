using Xunit;

namespace SolarMonitor.Data.UnitTests
{
    // see example explanation on xUnit.net website:
    // https://xunit.github.io/docs/getting-started-dnx.html
    public class SampleTest
    {
        [Fact]
        public void PassingTest()
        {
            Assert.Equal(4, Add(2, 2));
        }

        [Fact]
        public void PassingTest2()
        {
            Assert.Equal(3, Add(1, 2));
        }

        int Add(int x, int y)
        {
            return x + y;
        }
    }
}
