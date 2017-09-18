using System;
using System.Reflection;
using Xunit.Sdk;

namespace SolarMonitor.Api.IntegrationTests
{
    public class LogTestToConsoleAttribute : BeforeAfterTestAttribute
    {
        public override void Before(MethodInfo methodUnderTest)
        {
            Console.WriteLine($"    + {methodUnderTest.DeclaringType}.{methodUnderTest.Name}");
        }
        public override void After(MethodInfo methodUnderTest)
        {
            //Console.WriteLine("    - " + methodUnderTest.Name);
        }
    }
}