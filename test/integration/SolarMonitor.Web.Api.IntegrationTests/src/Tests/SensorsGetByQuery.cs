
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

using Resources = SolarMonitor.Data.Resources;
using Models = SolarMonitor.Data.Models;
using System.Linq;
using SolarMonitor.Data.Adapters;
using SolarMonitor.Data.CommonTypes;

namespace SolarMonitor.Api.IntegrationTests
{
    public class SensorsGetByQuery : SensorsTests
    {
        public SensorsGetByQuery()
        {
            Service.AuthToken = TokenProvider.UserToken;
        }

        private Resources.CollectionResource<Resources.Sensor> ConvertToResourceCollection(
            List<Models.Sensor> Sensors,
            int pageIndex = 1,
            int pageSize = 10)
        {
            var collection = new Resources.CollectionResource<Resources.Sensor>(TestConstants.SensorsPrefix,
                pageIndex, pageSize, Sensors.Count);

            var SensorAdapter = (ISensorAdapter)ServiceProvider.GetService(typeof(ISensorAdapter));

            var selectedSensors = Sensors.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            foreach (var Sensor in selectedSensors)
            {
                collection.AddItem(SensorAdapter.ModelToResource(Sensor));
            }
            return collection;
        }

        [Fact]
        public async Task GetByQueryWithoutAuthExpectUnauthorized()
        {
            // Arrange
            GenerateDefaultSensors();

            // Act
            Service.AuthToken = TokenProvider.EmptyToken;
            await Service.Get("", HttpStatusCode.Unauthorized);

            // Assert
        }

        [Fact]
        public async Task GetByQueryWithInvalidAuthExpectUnauthorized()
        {
            // Arrange
            GenerateDefaultSensors();

            // Act
            Service.AuthToken = TokenProvider.InvalidToken;
            await Service.Get("", HttpStatusCode.Unauthorized);

            // Assert
        }

        [Fact]
        public async Task GetByQueryWithAdminPermissionsExpectOK()
        {
            // Arrange
            var sensors = GenerateDefaultSensors();

            // Act
            Service.AuthToken = TokenProvider.AdminToken;
            var returnedSensors = await Service.Get("", HttpStatusCode.OK);

            // Assert
            returnedSensors.ShouldBeEquivalentTo(ConvertToResourceCollection(sensors));
        }



        [Fact]
        public async Task GetByQueryWithInvalidPageIndexExpectBadRequest()
        {
            // Arrange
            GenerateDefaultSensors();

            // Act
            await Service.Get("?pageIndex=aaa", HttpStatusCode.BadRequest);

            // Assert
            // nothing to do: parsing errors will be returned in the payload
        }

        [Fact]
        public async Task GetByQueryWithInvalidPageSizeExpectBadRequest()
        {
            // Arrange
            GenerateDefaultSensors();

            // Act
            await Service.Get("?pageIndex=1&pageSize=aa", HttpStatusCode.BadRequest);

            // Assert
            // nothing to do: parsing errors will be returned in the payload
        }

        [Fact]
        public async Task GetByQueryWithSingleSensorExpectOK()
        {
            // Arrange
            var sensors = _db.GenerateSensors(1, 1);

            // Act
            var returnedSensors = await Service.Get("", HttpStatusCode.OK);

            // Assert
            returnedSensors.ShouldBeEquivalentTo(ConvertToResourceCollection(sensors));
        }

        [Fact]
        public async Task GetByQueryMultipleSensorsExpectOK()
        {
            // Arrange
            var sensors = _db.GenerateSensors(2, 10);

            // Act
            var returnedSensors = await Service.Get("", HttpStatusCode.OK);

            // Assert
            returnedSensors.ShouldBeEquivalentTo(ConvertToResourceCollection(sensors));
        }


        [Fact]
        public async Task GetAllNoDataExpectNoContent()
        {
            // Arrange
            // leave database empty

            // Act
            var data = await Service.Get("", HttpStatusCode.NoContent);

            // Assert
            Assert.Equal(null, data);
        }

        [Fact]
        public async Task GetWithPageIndexAndPageSizeNoData()
        {
            // Arrange
            // leave database empty

            // Act
            var data = await Service.Get("?pageIndex=3&pageSize=5", HttpStatusCode.NoContent);

            // Assert
            Assert.Equal(null, data);

        }

        [Fact]
        public async Task GetAllExpectOK()
        {
            // Arrange
            var sensors = _db.GenerateSensors(2, 3);

            // Act
            var returnedSensors = await Service.Get();

            // Assert
            returnedSensors.ShouldBeEquivalentTo(ConvertToResourceCollection(sensors));
        }

        [Fact]
        public async Task GetWithPageSizeExpectOK()
        {
            // Arrange
            var sensors = _db.GenerateSensors(2, 2);

            // Act
            var returnedSensors = await Service.Get("?pageSize=2");

            // Assert
            returnedSensors.ShouldBeEquivalentTo(ConvertToResourceCollection(sensors, 1, 2));
        }

        [Fact]
        public async Task GetWithPageIndexAndPageSizeExpectOK()
        {
            // Arrange
            var sensors = _db.GenerateSensors(2, 4);

            // Act
            var returnedSensors = await Service.Get("?pageIndex=3&pageSize=2");

            // Assert
            returnedSensors.ShouldBeEquivalentTo(ConvertToResourceCollection(sensors, 3, 2));
        }

        [Fact]
        public async Task GetWithPageIndexExpectOK()
        {
            // Arrange
            var sensors = _db.GenerateSensors(3, 10);

            // Act
            var returnedSensors = await Service.Get("?pageIndex=2");

            // Assert
            returnedSensors.ShouldBeEquivalentTo(ConvertToResourceCollection(sensors, 2));
        }

        [Fact]
        public async Task GetWithType()
        {
            // Arrange
            var shunts = _db.GenerateSensors(3, 2, startingSensorId: 1, sensorType: SensorType.Shunt);
            var tempSensors = _db.GenerateSensors(3, 5, startingSensorId: 7, sensorType: SensorType.TemperatureSensor, createSites: false, createSensorTypes: false);
            var humSensors = _db.GenerateSensors(3, 3, startingSensorId: 23, sensorType: SensorType.HumiditySensor, createSites: false, createSensorTypes: false);

            // Act
            var returnedSensors = await Service.GetSensors("?type=TemperatureSensor");

            // Assert
            returnedSensors.ShouldBeEquivalentTo(ConvertToResourceCollection(tempSensors));
        }
    }
}