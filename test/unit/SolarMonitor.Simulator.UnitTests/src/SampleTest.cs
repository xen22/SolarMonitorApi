using Xunit;

namespace SolarMonitor.Simulator.UnitTests
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

        public void PassingTest2()
        {
            Assert.Equal(4, Add(1, 3));
        }

        int Add(int x, int y)
        {
            return x + y;
        }
    }
}
