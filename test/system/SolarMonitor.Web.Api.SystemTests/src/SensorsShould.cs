using System;
using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using System.Reflection;

namespace SolarMonitor.Api.IntegrationTests
{

  public class SensorsShould
  {
    private readonly HttpClient _client;

    public SensorsShould()
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
    public async Task Return45Sensors()
    {
      //Arrange 
      string expectedVersion = this.GetType().GetTypeInfo().Assembly.GetName().Version.ToString();

      // Act
      var response = await _client.GetAsync("/api/sensors");
      response.EnsureSuccessStatusCode();

      var responseString = await response.Content.ReadAsStringAsync();

      // Assert
      Assert.Equal(expectedVersion, responseString);
    }
  }
}
