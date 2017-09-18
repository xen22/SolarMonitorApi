
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

using Resources = SolarMonitor.Data.Resources;
using Models = SolarMonitor.Data.Models;
using SolarMonitor.Data.CommonTypes;

namespace SolarMonitor.Api.IntegrationTests
{
    public class SensorsCreate : SensorsTests
    {
        public SensorsCreate()
        {
            // Note: the Sensors.Create() action needs admin privileges so by default we're using an AdminToken
            Service.AuthToken = TokenProvider.AdminToken;
        }

        private Resources.Sensor NewSensor(SensorType type = SensorType.Shunt, int id = 99, string siteName = "Site1")
        {
            var sensorFactory = new ResourceSensorFactory();
            var s = sensorFactory.CreateSensor(type, id, siteName);
            return s;
        }

        private void CheckSensorCreatedInDatabase(Resources.Sensor s)
        {
            // check that the new Sensor has been added to the database
            var newModel = _db.GetSensor(s.Guid);
            Assert.NotNull(newModel);
            newModel.ShouldBeEquivalentTo(ConvertToModel(s), opt => opt.Excluding(ctx => ctx.SelectedMemberInfo.Name == "Id"));
        }

        // Compares to Resource.Sensor objects but ignores Links property. 
        private void SensorsShouldBeEquivallent(Resources.Sensor testVal, Resources.Sensor expectedVal)
        {
            var newSensor = testVal;
            newSensor.Links.Clear();
            newSensor.ShouldBeEquivalentTo(expectedVal);
        }

        private Resources.Sensor RemoveLinksFromSensorResource(Resources.Sensor createdSensor)
        {
            var newSensor = createdSensor;
            newSensor.Links.Clear();
            return newSensor;
        }

        [Fact]
        public async Task CreateWithoutAuthExpectUnauthorized()
        {
            // Arrange
            GenerateDefaultSensors();

            // Act
            var s = NewSensor();
            Service.AuthToken = TokenProvider.EmptyToken;
            var data = await Service.Create(s, HttpStatusCode.Unauthorized);

            // Assert
            Assert.Equal(null, data);
        }

        [Fact]
        public async Task CreateWithInvalidAuthExpectUnauthorized()
        {
            // Arrange
            GenerateDefaultSensors();

            // Act
            Service.AuthToken = TokenProvider.InvalidToken;
            var data = await Service.Create(NewSensor(), HttpStatusCode.Unauthorized);

            // Assert
            Assert.Equal(null, data);
        }

        [Fact]
        public async Task CreateWithNoPermissionsExpectForbidden()
        {
            // Arrange
            GenerateDefaultSensors();

            // Act
            Service.AuthToken = TokenProvider.UserToken;
            var data = await Service.Create(NewSensor(), HttpStatusCode.Forbidden);

            // Assert
            Assert.Equal(null, data);
        }


        [Fact]
        public async Task CreateWithInvalidDataExpectBadRequest()
        {
            // Arrange
            GenerateDefaultSensors();

            // Act
            var data = await Service.Create("random data", HttpStatusCode.BadRequest);

            // Assert
            // nothing to do: parsing errors will be returned in the payload
        }

        [Fact]
        public async Task CreateWithMissingTypeExpectBadRequest()
        {
            // Arrange
            GenerateDefaultSensors();

            // Act
            var data = await Service.Create("{'Name' = 'Shunt1', 'Site': 'Site1', Description': 'test'}", HttpStatusCode.BadRequest);

            // Assert
        }

        [Fact]
        public async Task CreateWithMissingNameExpectBadRequest()
        {
            // Arrange
            GenerateDefaultSensors();

            // Act
            var data = await Service.Create("{'Type' = 'Shunt', 'Site': 'Site1', Description': 'test'}", HttpStatusCode.BadRequest);

            // Assert
        }

        [Fact]
        public async Task CreateWithMissingDescriptionExpectCreated()
        {
            // Arrange
            GenerateDefaultSensors();

            // Act
            string guid = "988151e8-93bb-11e7-a77a-0021cc700a6e";
            var createdSensor = await Service.Create(
                $"{{'type': 'Shunt', 'name': 'TestSensor99', 'site': 'Site1', 'guid': '{guid}'}}", HttpStatusCode.Created);

            // Assert
            Assert.Equal("Shunt", createdSensor.Type);
            Assert.Equal("Site1", createdSensor.Site);
            Assert.Equal("TestSensor99", createdSensor.Name);
            Assert.Equal(null, createdSensor.Description);
            Assert.Equal(Guid.Parse(guid), createdSensor.Guid);
            CheckSensorCreatedInDatabase(createdSensor);
        }

        [Fact]
        public async Task CreateWithInvalidSiteExpectBadRequest()
        {
            // Arrange

            // Act
            var s = NewSensor(SensorType.Shunt, 100, "Site99"); // Site99 does not exist
            var createdSensor = await Service.Create(s, HttpStatusCode.BadRequest);

            // Assert
        }

        [Fact]
        public async Task CreateWithNoExistingSensorsExpectCreated()
        {
            // Arrange
            _db.GenerateSensorTypes();
            _db.GenerateSites(1);

            // Act
            var s = NewSensor();
            var createdSensor = await Service.Create(s, HttpStatusCode.Created);

            // Assert
            SensorsShouldBeEquivallent(createdSensor, s);
            CheckSensorCreatedInDatabase(s);
        }

        [Fact]
        public async Task CreateExpectCreated()
        {
            // Arrange
            GenerateDefaultSensors();

            // Act
            var s = NewSensor();
            var createdSensor = await Service.Create(s, HttpStatusCode.Created);

            // Assert
            SensorsShouldBeEquivallent(createdSensor, s);
            CheckSensorCreatedInDatabase(s);
        }

        [Fact]
        public async Task CreateAllTypesExpectCreated()
        {
            // Arrange
            GenerateDefaultSensors();

            foreach (SensorType type in Enum.GetValues(typeof(SensorType)))
            {
                if (type == SensorType.Unset)
                {
                    continue;
                }
                // Act
                var s = NewSensor(type);
                var createdSensor = await Service.Create(s, HttpStatusCode.Created);

                // Assert
                SensorsShouldBeEquivallent(createdSensor, s);
                CheckSensorCreatedInDatabase(s);
            }
        }

        [Fact]
        public async Task CreateAndGetExpectCreatedThenOK()
        {
            // Arrange
            GenerateDefaultSensors();

            // Act
            var s = NewSensor();
            var createdSensor = await Service.Create(s, HttpStatusCode.Created);
            var createdSensor2 = await Service.GetSingle(createdSensor.Guid, HttpStatusCode.OK);

            // Assert
            SensorsShouldBeEquivallent(createdSensor, s);
            SensorsShouldBeEquivallent(createdSensor2, s);
            CheckSensorCreatedInDatabase(s);
        }

        [Fact]
        public async Task CreateWithoutGuidExpectCreated()
        {
            // Arrange
            GenerateDefaultSensors();

            // Act
            var s = new Resources.Shunt { Name = "Hello", Site = "Site1", Type = "Shunt" };
            var createdSensor = await Service.Create(s, HttpStatusCode.Created);

            // Assert
            Assert.Equal(s.Name, createdSensor.Name);
            Assert.NotEqual(Guid.Empty, createdSensor.Guid);
            CheckSensorCreatedInDatabase(createdSensor);
        }


    }
}