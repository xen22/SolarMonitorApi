
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
    public class SitesDelete : SitesTests
    {
        public SitesDelete()
        {
            // we need token with admin role in order to delete sites
            Service.AuthToken = TokenProvider.AdminToken;
        }

        private void CheckSiteDoesNotExistInDatabase(Guid guid)
        {
            var newModel = _db.GetSite(guid);
            Assert.Null(newModel);
        }

        protected void CheckSiteExistsInDatabase(Guid guid)
        {
            var newModel = _db.GetSite(guid);
            Assert.NotNull(newModel);
        }


        [Fact]
        public async Task DeleteWithoutAuthExpectUnauthorized()
        {
            // Arrange
            var sites = _db.GenerateSites(3);

            // Act
            Service.AuthToken = TokenProvider.EmptyToken;
            await Service.Delete(sites[0].Guid, HttpStatusCode.Unauthorized);

            // Assert
            CheckSiteExistsInDatabase(sites[0].Guid);
        }

        [Fact]
        public async Task DeleteWithInvalidAuthExpectUnauthorized()
        {
            // Arrange
            var sites = _db.GenerateSites(3);

            // Act
            Service.AuthToken = TokenProvider.InvalidToken;
            await Service.Delete(sites[0].Guid, HttpStatusCode.Unauthorized);

            // Assert
            CheckSiteExistsInDatabase(sites[0].Guid);
        }

        [Fact]
        public async Task DeleteWithNoPermissionsExpectForbidden()
        {
            // Arrange
            var sites = _db.GenerateSites(3);

            // Act
            Service.AuthToken = TokenProvider.UserToken;
            await Service.Delete(sites[0].Guid, HttpStatusCode.Forbidden);

            // Assert
            CheckSiteExistsInDatabase(sites[0].Guid);
        }

        [Fact]
        public async Task DeleteWithInvalidGuidExpectBadRequest()
        {
            // Arrange
            _db.GenerateSites(3);

            // Act
            await Service.Delete("invalid-guid", HttpStatusCode.BadRequest);

            // Assert
            // nothing to do: parsing errors will be returned in the payload
        }

        [Fact]
        public async Task DeleteWithNonExistingGuidExpectNotFound()
        {
            // Arrange
            _db.GenerateSites(3);

            // Act
            await Service.Delete(Guid.NewGuid(), HttpStatusCode.NotFound);

            // Assert
            // nothing to do: parsing errors will be returned in the payload
        }

        [Fact]
        public async Task DeleteWithSingleSiteExpectNoContent()
        {
            // Arrange
            var sites = _db.GenerateSites(1);

            // Act
            await Service.Delete(sites[0].Guid, HttpStatusCode.NoContent);

            // Assert
            CheckSiteDoesNotExistInDatabase(sites[0].Guid);
        }

        [Fact]
        public async Task DeleteBeginningExpectNoContent()
        {
            // Arrange
            var sites = _db.GenerateSites(3);

            // Act
            await Service.Delete(sites[0].Guid, HttpStatusCode.NoContent);

            // Assert
            CheckSiteDoesNotExistInDatabase(sites[0].Guid);
            CheckSiteExistsInDatabase(sites[1].Guid);
            CheckSiteExistsInDatabase(sites[2].Guid);
        }

        [Fact]
        public async Task DeleteEndExpectNoContent()
        {
            // Arrange
            var sites = _db.GenerateSites(3);

            // Act
            await Service.Delete(sites[2].Guid, HttpStatusCode.NoContent);

            // Assert
            CheckSiteExistsInDatabase(sites[0].Guid);
            CheckSiteExistsInDatabase(sites[1].Guid);
            CheckSiteDoesNotExistInDatabase(sites[2].Guid);
        }

        [Fact]
        public async Task DeleteMiddleExpectNoContent()
        {
            // Arrange
            var sites = _db.GenerateSites(3);

            // Act
            await Service.Delete(sites[1].Guid, HttpStatusCode.NoContent);

            // Assert
            CheckSiteDoesNotExistInDatabase(sites[1].Guid);
            CheckSiteExistsInDatabase(sites[0].Guid);
            CheckSiteExistsInDatabase(sites[2].Guid);
        }


        [Fact]
        public async Task DeleteMultipleSitesExpectNoContent()
        {
            // Arrange
            var sites = _db.GenerateSites(3);

            // Act
            foreach (var site in sites)
            {
                await Service.Delete(site.Guid, HttpStatusCode.NoContent);
            }

            // Assert
            sites.ForEach(s => CheckSiteDoesNotExistInDatabase(s.Guid));
        }

        [Fact]
        public async Task DeleteSiteMultipleTimesExpectNoContentThenNotFound()
        {
            // Arrange
            var sites = _db.GenerateSites(3);

            // Act
            await Service.Delete(sites[1].Guid, HttpStatusCode.NoContent);
            await Service.Delete(sites[1].Guid, HttpStatusCode.NotFound);

            // Assert
            CheckSiteDoesNotExistInDatabase(sites[1].Guid);
            CheckSiteExistsInDatabase(sites[0].Guid);
            CheckSiteExistsInDatabase(sites[2].Guid);
        }

        [Fact]
        public async Task DeleteSiteThenGetExpectNoContentThenNotFound()
        {
            // Arrange
            var sites = _db.GenerateSites(3);

            // Act
            await Service.Delete(sites[1].Guid, HttpStatusCode.NoContent);
            var returnedSite = await Service.GetSingle(sites[1].Guid, HttpStatusCode.NotFound);

            // Assert
            Assert.Null(returnedSite);
            CheckSiteDoesNotExistInDatabase(sites[1].Guid);
            CheckSiteExistsInDatabase(sites[0].Guid);
            CheckSiteExistsInDatabase(sites[2].Guid);
        }

    }
}