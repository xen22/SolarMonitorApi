
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
    public class SitesGetByGuid : SitesTests
    {
        public SitesGetByGuid()
        {
            Service.AuthToken = TokenProvider.UserToken;
        }

        [Fact]
        public async Task GetByGuidWithoutAuthExpectUnauthorized()
        {
            // Arrange
            var sites = _db.GenerateSites(3);

            // Act
            Service.AuthToken = TokenProvider.EmptyToken;
            await Service.GetSingle(sites[0].Guid, HttpStatusCode.Unauthorized);

            // Assert
        }

        [Fact]
        public async Task GetByGuidWithInvalidAuthExpectUnauthorized()
        {
            // Arrange
            var sites = _db.GenerateSites(3);

            // Act
            Service.AuthToken = TokenProvider.InvalidToken;
            await Service.GetSingle(sites[0].Guid, HttpStatusCode.Unauthorized);

            // Assert
        }

        [Fact]
        public async Task GetByGuidWithAdminPermissionsExpectOK()
        {
            // Arrange
            var sites = _db.GenerateSites(3);

            // Act
            Service.AuthToken = TokenProvider.AdminToken;
            var returnedSite = await Service.GetSingle(sites[0].Guid, HttpStatusCode.OK);

            // Assert
            returnedSite.ShouldBeEquivalentTo(ConvertToResource(sites[0]));
        }

        [Fact]
        public async Task GetByGuidWithInvalidGuidExpectBadRequest()
        {
            // Arrange
            _db.GenerateSites(3);

            // Act
            await Service.GetSingle("invalid-guid", HttpStatusCode.BadRequest);

            // Assert
            // nothing to do: parsing errors will be returned in the payload
        }

        [Fact]
        public async Task GetByGuidWithNonExistingGuidExpectNotFound()
        {
            // Arrange
            _db.GenerateSites(3);

            // Act
            var data = await Service.GetSingle(Guid.NewGuid(), HttpStatusCode.NotFound);

            // Assert
            Assert.Null(data);
        }

        [Fact]
        public async Task GetByGuidWithSingleSiteExpectOK()
        {
            // Arrange
            var sites = _db.GenerateSites(1);

            // Act
            var returnedSite = await Service.GetSingle(sites[0].Guid, HttpStatusCode.OK);

            // Assert
            returnedSite.ShouldBeEquivalentTo(ConvertToResource(sites[0]));
        }

        [Fact]
        public async Task GetByGuidMultipleSitesExpectOK()
        {
            // Arrange
            var sites = _db.GenerateSites(3);

            foreach (var site in sites)
            {
                // Act
                var returnedSite = await Service.GetSingle(site.Guid, HttpStatusCode.OK);
                // Assert
                returnedSite.ShouldBeEquivalentTo(ConvertToResource(site));
            }

        }

        [Fact]
        public async Task GetByGuidSiteMultipleTimesExpectOK()
        {
            // Arrange
            var sites = _db.GenerateSites(3);

            // Act
            var returnedSite = await Service.GetSingle(sites[1].Guid, HttpStatusCode.OK);
            var returnedSite2 = await Service.GetSingle(sites[1].Guid, HttpStatusCode.OK);

            // Assert
            returnedSite.ShouldBeEquivalentTo(ConvertToResource(sites[1]));
            returnedSite2.ShouldBeEquivalentTo(ConvertToResource(sites[1]));
        }

    }
}