using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SolarMonitor.Data.Resources;
using Xunit;
using FluentAssertions;
using SolarMonitor.Api.IntegrationTests.Services;

using Resources = SolarMonitor.Data.Resources;
using Models = SolarMonitor.Data.Models;
using SolarMonitor.Data.Adapters;

namespace SolarMonitor.Api.IntegrationTests
{
    //[Collection("Sensor tests")]
    public class SensorsTests : IntegrationTestHelper, IDisposable
    {
        protected SensorService Service { get; private set; }
        protected ISensorAdapter SensorAdapter { get; private set; }
        public SensorsTests()
        {
            Service = new SensorService(_client, TokenProvider);
            SensorAdapter = (ISensorAdapter)ServiceProvider.GetService(typeof(ISensorAdapter));
            System.Console.WriteLine("-------- Begin");
        }

        public virtual void Dispose()
        {
            System.Console.WriteLine("-------- End");
            Service.AuthToken = TokenProvider.UserToken;
            _db.RecreateDatabase();
        }

        protected Resources.Sensor ConvertToResource(Models.Sensor Sensor)
        {
            return SensorAdapter.ModelToResource(Sensor);
        }

        protected Models.Sensor ConvertToModel(Resources.Sensor s)
        {
            return SensorAdapter.ResourceToModel(s);
        }

        // add some default sensors to the database - this is used when we want to add some data
        // but we don't need to test the sensors added explicitly
        protected List<Models.Sensor> GenerateDefaultSensors()
        {
            return _db.GenerateSensors(2, 3);
        }

    }
}

