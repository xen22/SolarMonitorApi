
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

using Resources = SolarMonitor.Data.Resources;
using Models = SolarMonitor.Data.Models;
using System.Linq;

namespace SolarMonitor.Api.IntegrationTests
{
    public class SensorsDelete : SensorsTests
    {
        public SensorsDelete()
        {
            // we need token with admin role in order to delete Sensors
            Service.AuthToken = TokenProvider.AdminToken;
        }

        private void CheckSensorDoesNotExistInDatabase(Guid guid)
        {
            System.Console.WriteLine("Num sensors left: " + _db.NumSensors());
            var newModel = _db.GetSensor(guid);
            Assert.Null(newModel);
        }

        protected void CheckSensorExistsInDatabase(Guid guid)
        {
            var newModel = _db.GetSensor(guid);
            Assert.NotNull(newModel);
        }

        [Fact]
        public async Task DeleteWithoutAuthExpectUnauthorized()
        {
            // Arrange
            var sensors = GenerateDefaultSensors();

            // Act
            Service.AuthToken = TokenProvider.EmptyToken;
            await Service.Delete(sensors[0].Guid, HttpStatusCode.Unauthorized);

            // Assert
            CheckSensorExistsInDatabase(sensors[0].Guid);
        }

        [Fact]
        public async Task DeleteWithInvalidAuthExpectUnauthorized()
        {
            // Arrange
            var sensors = GenerateDefaultSensors();

            // Act
            Service.AuthToken = TokenProvider.InvalidToken;
            await Service.Delete(sensors[0].Guid, HttpStatusCode.Unauthorized);

            // Assert
            CheckSensorExistsInDatabase(sensors[0].Guid);
        }

        [Fact]
        public async Task DeleteWithNoPermissionsExpectForbidden()
        {
            // Arrange
            var sensors = GenerateDefaultSensors();

            // Act
            Service.AuthToken = TokenProvider.UserToken;
            await Service.Delete(sensors[0].Guid, HttpStatusCode.Forbidden);

            // Assert
            CheckSensorExistsInDatabase(sensors[0].Guid);
        }

        [Fact]
        public async Task DeleteWithInvalidGuidExpectBadRequest()
        {
            // Arrange
            GenerateDefaultSensors();

            // Act
            await Service.Delete("invalid-guid", HttpStatusCode.BadRequest);

            // Assert
            // nothing to do: parsing errors will be returned in the payload
        }

        [Fact]
        public async Task DeleteWithNonExistingGuidExpectNotFound()
        {
            // Arrange
            GenerateDefaultSensors();

            // Act
            await Service.Delete(Guid.NewGuid(), HttpStatusCode.NotFound);

            // Assert
            // nothing to do: parsing errors will be returned in the payload
        }

        [Fact]
        public async Task DeleteWithSingleSensorExpectNoContent()
        {
            // Arrange
            var sensors = _db.GenerateSensors(1, 1);

            // Act
            await Service.Delete(sensors[0].Guid, HttpStatusCode.NoContent);

            // Assert
            CheckSensorDoesNotExistInDatabase(sensors[0].Guid);
        }

        [Fact]
        public async Task DeleteBeginningExpectNoContent()
        {
            // Arrange
            var sensors = _db.GenerateSensors(2, 4);

            // Act
            await Service.Delete(sensors[0].Guid, HttpStatusCode.NoContent);

            // Assert
            CheckSensorDoesNotExistInDatabase(sensors[0].Guid);
        }

        [Fact]
        public async Task DeleteEndExpectNoContent()
        {
            // Arrange
            var sensors = _db.GenerateSensors(2, 4);

            // Act
            await Service.Delete(sensors[7].Guid, HttpStatusCode.NoContent);

            // Assert
            CheckSensorDoesNotExistInDatabase(sensors[7].Guid);
        }

        [Fact]
        public async Task DeleteMiddleExpectNoContent()
        {
            // Arrange
            var sensors = _db.GenerateSensors(2, 4);

            // Act
            await Service.Delete(sensors[4].Guid, HttpStatusCode.NoContent);

            // Assert
            CheckSensorDoesNotExistInDatabase(sensors[4].Guid);
        }


        [Fact]
        public async Task DeleteMultipleSensorsExpectNoContent()
        {
            // Arrange
            var sensors = _db.GenerateSensors(2, 4);

            // Act
            //sensors.ForEach(async s => await Service.Delete(s.Guid, HttpStatusCode.NoContent));
            foreach (var s in sensors)
            {
                await Service.Delete(s.Guid, HttpStatusCode.NoContent);
            }

            // Assert
            sensors.ForEach(s => CheckSensorDoesNotExistInDatabase(s.Guid));
        }

        [Fact(Skip = "Concurrent requests not currently supported by the API")]
        public async Task DeleteMultipleSensorsConcurrentlyExpectNoContent()
        {
            // Arrange
            var sensors = _db.GenerateSensors(2, 4);

            // Act
            await Task.WhenAll(sensors.Select(s => Service.Delete(s.Guid, HttpStatusCode.NoContent)));

            // Assert
            sensors.ForEach(s => CheckSensorDoesNotExistInDatabase(s.Guid));
        }

        [Fact]
        public async Task DeleteSensorMultipleTimesExpectNoContentThenNotFound()
        {
            // Arrange
            var sensors = _db.GenerateSensors(2, 4);

            // Act
            await Service.Delete(sensors[1].Guid, HttpStatusCode.NoContent);
            await Service.Delete(sensors[1].Guid, HttpStatusCode.NotFound);

            // Assert
            CheckSensorDoesNotExistInDatabase(sensors[1].Guid);
        }

        [Fact]
        public async Task DeleteSensorThenGetExpectNoContentThenNotFound()
        {
            // Arrange
            var sensors = _db.GenerateSensors(2, 4);

            // Act
            await Service.Delete(sensors[1].Guid, HttpStatusCode.NoContent);
            var returnedSensor = await Service.GetSingle(sensors[1].Guid, HttpStatusCode.NotFound);

            // Assert
            Assert.Null(returnedSensor);
        }

    }
}