using System;
using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using System.Reflection;

namespace SolarMonitor.Api.IntegrationTests
{

  public class SolarMonitorShould
  {
    private readonly HttpClient _client;

    public SolarMonitorShould()
    {
      // Arrange
      _client = new HttpClient();
      // Note: This is needed to support automatic testing in Jenkins via docker (see Jenkinsfile) 
      string ipAddr = Environment.GetEnvironmentVariable("WEB_API_CONTAINER_IPP_ADDR");
      if(ipAddr == "") {
        ipAddr = "172.17.0.2";
      }
      System.Console.WriteLine("SolarMonitorShould: SUT IP address set to " + ipAddr);
      _client.BaseAddress = new Uri("http://" + ipAddr);
    }

    [Fact]
    public async Task ReturnOwnVersion()
    {
      //Arrange 
      // Note: the version returned by the Web API service is the version of the SolarMonitor.Web.Api.dll assembly
      //   but since all assemblies get versioned automatically by the Jenkins build, we can get the same version
      //   from the current integration test assembly.
      string expectedVersion = this.GetType().GetTypeInfo().Assembly.GetName().Version.ToString();

      // Act
      var response = await _client.GetAsync("/api/SolarMonitor/version");
      response.EnsureSuccessStatusCode();

      var responseString = await response.Content.ReadAsStringAsync();

      // Assert
      Assert.Equal(expectedVersion, responseString);
    }
  }
}
