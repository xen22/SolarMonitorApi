
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
    public class SitesCreate : SitesTests
    {
        public SitesCreate()
        {
            // Note: the Sites.Create() action needs admin privileges so by default we're using an AdminToken
            Service.AuthToken = TokenProvider.AdminToken;
        }

        private Resources.Site NewSite()
        {
            return new Resources.Site { Name = "NewSite99", Id = 99, Guid = Guid.NewGuid() };
        }

        private void CheckSiteCreatedInDatabase(Resources.Site s)
        {
            // check that the new site has been added to the database
            var newModel = _db.GetSite(s.Id);
            Assert.NotNull(newModel);
            newModel.ShouldBeEquivalentTo(ConvertToModel(s));
        }

        // Compares to Resource.Site objects but ignores Links property. 
        private void SitesShouldBeEquivallent(Resources.Site testVal, Resources.Site expectedVal)
        {
            var newSite = testVal;
            newSite.Links.Clear();
            newSite.ShouldBeEquivalentTo(expectedVal);
        }

        private Resources.Site RemoveLinksFromSiteResource(Resources.Site createdSite)
        {
            var newSite = createdSite;
            newSite.Links.Clear();
            return newSite;
        }

        [Fact]
        public async Task CreateWithoutAuthExpectUnauthorized()
        {
            // Arrange
            _db.GenerateSites(3);

            // Act
            var s = NewSite();
            Service.AuthToken = TokenProvider.EmptyToken;
            var data = await Service.Create(s, HttpStatusCode.Unauthorized);

            // Assert
            Assert.Equal(null, data);
        }

        [Fact]
        public async Task CreateWithInvalidAuthExpectUnauthorized()
        {
            // Arrange
            _db.GenerateSites(3);

            // Act
            Service.AuthToken = TokenProvider.InvalidToken;
            var data = await Service.Create(NewSite(), HttpStatusCode.Unauthorized);

            // Assert
            Assert.Equal(null, data);
        }

        [Fact]
        public async Task CreateWithNoPermissionsExpectForbidden()
        {
            // Arrange
            _db.GenerateSites(3);

            // Act
            Service.AuthToken = TokenProvider.UserToken;
            var data = await Service.Create(NewSite(), HttpStatusCode.Forbidden);

            // Assert
            Assert.Equal(null, data);
        }


        [Fact]
        public async Task CreateWithInvalidDataExpectBadRequest()
        {
            // Arrange
            _db.GenerateSites(3);

            // Act
            var data = await Service.Create("random data", HttpStatusCode.BadRequest);

            // Assert
            // nothing to do: parsing errors will be returned in the payload
        }

        [Fact]
        public async Task CreateWithMissingNameExpectBadRequest()
        {
            // Arrange
            _db.GenerateSites(3);

            // Act
            var data = await Service.Create("{'Description': 'test'}", HttpStatusCode.BadRequest);

            // Assert
        }

        [Fact]
        public async Task CreateWithMissingTimezoneExpectCreated()
        {
            // Arrange
            _db.GenerateSites(3);

            // Act
            string guid = "988151e8-93bb-11e7-a77a-0021cc700a6e";
            var createdSite = await Service.Create($"{{'Name': 'TestSite99', 'Guid': '{guid}'}}", HttpStatusCode.Created);

            // Assert
            // we need to only check the name and timezone
            Assert.Equal("TestSite99", createdSite.Name);
            Assert.Equal(null, createdSite.Timezone);
            Assert.NotEqual(0, createdSite.Id);
            Assert.Equal(Guid.Parse(guid), createdSite.Guid);
        }

        [Fact]
        public async Task CreateWithNoExistingExpectCreated()
        {
            // Arrange

            // Act
            var s = NewSite();
            var createdSite = await Service.Create(s, HttpStatusCode.Created);

            // Assert
            SitesShouldBeEquivallent(createdSite, s);
            CheckSiteCreatedInDatabase(s);
        }

        [Fact]
        public async Task CreateExpectCreated()
        {
            // Arrange
            _db.GenerateSites(3);

            // Act
            var s = NewSite();
            var createdSite = await Service.Create(s, HttpStatusCode.Created);

            // Assert
            SitesShouldBeEquivallent(createdSite, s);
            CheckSiteCreatedInDatabase(s);
        }

        [Fact]
        public async Task CreateAndGetExpectCreatedThenOK()
        {
            // Arrange
            _db.GenerateSites(3);

            // Act
            var s = NewSite();
            var createdSite = await Service.Create(s, HttpStatusCode.Created);
            var createdSite2 = await Service.GetSingle(createdSite.Guid, HttpStatusCode.OK);

            // Assert
            SitesShouldBeEquivallent(createdSite, s);
            SitesShouldBeEquivallent(createdSite2, s);
            CheckSiteCreatedInDatabase(s);
        }

        [Fact]
        public async Task CreateWithoutIdExpectCreated()
        {
            // Arrange
            _db.GenerateSites(3);

            // Act
            var s = new Resources.Site { Name = "Hello", Timezone = "NZST", Guid = Guid.NewGuid() };
            var createdSite = await Service.Create(s, HttpStatusCode.Created);

            // Assert
            Assert.Equal(4, createdSite.Id);
            Assert.Equal(s.Name, createdSite.Name);
            Assert.Equal(s.Timezone, createdSite.Timezone);
            Assert.Equal(s.Guid, createdSite.Guid);
            CheckSiteCreatedInDatabase(createdSite);
        }

        [Fact(Skip = "Problem with the test f/w: SQLite in-memory backend expects Guid to be set")]
        public async Task CreateWithoutIdAndGuidExpectCreated()
        {
            // Arrange
            _db.GenerateSites(3);

            // Act
            var s = new Resources.Site { Name = "Hello", Timezone = "NZST" };
            var createdSite = await Service.Create(s, HttpStatusCode.Created);

            // Assert
            Assert.Equal(4, createdSite.Id);
            Assert.Equal(s.Name, createdSite.Name);
            Assert.Equal(s.Timezone, createdSite.Timezone);
            Assert.NotEqual(Guid.Empty, createdSite.Guid);
            CheckSiteCreatedInDatabase(createdSite);
        }

        [Fact]
        public async Task CreateWithoutIdWhenNoneExistingExpectCreated()
        {
            // Arrange

            // Act
            var s = new Resources.Site { Name = "Hello", Timezone = "NZST", Guid = Guid.NewGuid() };
            var createdSite = await Service.Create(s, HttpStatusCode.Created);

            // Assert
            Assert.Equal(1, createdSite.Id);
            Assert.Equal(s.Name, createdSite.Name);
            Assert.Equal(s.Timezone, createdSite.Timezone);
            Assert.Equal(s.Guid, createdSite.Guid);
            CheckSiteCreatedInDatabase(createdSite);
        }

    }
}