using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Models = SolarMonitor.Data.Models;
using Resources = SolarMonitor.Data.Resources;

namespace SolarMonitor.Api.IntegrationTests.Services
{
    public class SensorService : ApiService<Models.Sensor, Resources.Sensor, Guid>
    {
        public SensorService(HttpClient client, TokenProvider tokenProvider) : base(client, "sensors", tokenProvider)
        {
        }

        public override async Task<Resources.Sensor> Create(
            string newEntity,
            HttpStatusCode expectedStatusCode = HttpStatusCode.Created)
        {
            var data = await CreateHelper(newEntity, expectedStatusCode);
            if (string.IsNullOrWhiteSpace(data))
            {
                return null;
            }

            var genericSensor = JsonConvert.DeserializeObject<Resources.Sensor>(data);

            var assemblyName = typeof(Resources.Sensor).GetTypeInfo().Assembly.ToString();
            var namespaceName = typeof(Resources.Sensor).Namespace;
            var typeName = $"{namespaceName}.{genericSensor.Type}, {assemblyName}";

            var sensorType = Type.GetType(typeName);
            var specialisedSensor = JsonConvert.DeserializeObject(data, sensorType);
            return (Resources.Sensor)specialisedSensor;
        }
    }

}