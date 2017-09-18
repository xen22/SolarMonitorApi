
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

using Resources = SolarMonitor.Data.Resources;
using Models = SolarMonitor.Data.Models;

namespace SolarMonitor.Api.IntegrationTests
{
    public class SensorsGetByGuid : SensorsTests
    {
        public SensorsGetByGuid()
        {
            Service.AuthToken = TokenProvider.UserToken;
        }

        [Fact]
        public async Task GetByGuidWithoutAuthExpectUnauthorized()
        {
            // Arrange
            var sensors = GenerateDefaultSensors();

            // Act
            Service.AuthToken = TokenProvider.EmptyToken;
            await Service.GetSingle(sensors[0].Guid, HttpStatusCode.Unauthorized);

            // Assert
        }

        [Fact]
        public async Task GetByGuidWithInvalidAuthExpectUnauthorized()
        {
            // Arrange
            var sensors = GenerateDefaultSensors();

            // Act
            Service.AuthToken = TokenProvider.InvalidToken;
            await Service.GetSingle(sensors[0].Guid, HttpStatusCode.Unauthorized);

            // Assert
        }

        [Fact]
        public async Task GetByGuidWithAdminPermissionsExpectOK()
        {
            // Arrange
            var sensors = GenerateDefaultSensors();

            // Act
            Service.AuthToken = TokenProvider.AdminToken;
            var returnedSensor = await Service.GetSingle(sensors[0].Guid, HttpStatusCode.OK);

            // Assert
            returnedSensor.ShouldBeEquivalentTo(ConvertToResource(sensors[0]));
        }

        [Fact]
        public async Task GetByGuidWithInvalidGuidExpectBadRequest()
        {
            // Arrange
            GenerateDefaultSensors();

            // Act
            await Service.GetSingle("invalid-guid", HttpStatusCode.BadRequest);

            // Assert
            // nothing to do: parsing errors will be returned in the payload
        }

        [Fact]
        public async Task GetByGuidWithNonExistingGuidExpectNotFound()
        {
            // Arrange
            GenerateDefaultSensors();

            // Act
            var data = await Service.GetSingle(Guid.NewGuid(), HttpStatusCode.NotFound);

            // Assert
            Assert.Null(data);
        }

        [Fact]
        public async Task GetByGuidWithSingleSensorExpectOK()
        {
            // Arrange
            var sensors = _db.GenerateSensors(1, 1);

            // Act
            var returnedSensor = await Service.GetSingle(sensors[0].Guid, HttpStatusCode.OK);

            // Assert
            returnedSensor.ShouldBeEquivalentTo(ConvertToResource(sensors[0]));
        }

        [Fact]
        public async Task GetByGuidMultipleSensorsExpectOK()
        {
            // Arrange
            var sensors = _db.GenerateSensors(2, 3);

            foreach (var Sensor in sensors)
            {
                // Act
                var returnedSensor = await Service.GetSingle(Sensor.Guid, HttpStatusCode.OK);
                // Assert
                returnedSensor.ShouldBeEquivalentTo(ConvertToResource(Sensor));
            }

        }

        [Fact]
        public async Task GetByGuidSensorMultipleTimesExpectOK()
        {
            // Arrange
            var sensors = _db.GenerateSensors(2, 3);

            // Act
            var returnedSensor = await Service.GetSingle(sensors[1].Guid, HttpStatusCode.OK);
            var returnedSensor2 = await Service.GetSingle(sensors[1].Guid, HttpStatusCode.OK);

            // Assert
            returnedSensor.ShouldBeEquivalentTo(ConvertToResource(sensors[1]));
            returnedSensor2.ShouldBeEquivalentTo(ConvertToResource(sensors[1]));
        }

    }
}