
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

namespace SolarMonitor.Api.IntegrationTests
{
    public class SitesGetByQuery : SitesTests
    {
        public SitesGetByQuery()
        {
            Service.AuthToken = TokenProvider.UserToken;
        }

        private Resources.CollectionResource<Resources.Site> ConvertToResourceCollection(
            List<Models.Site> sites,
            int pageIndex = 1,
            int pageSize = 10)
        {
            var collection = new Resources.CollectionResource<Resources.Site>(TestConstants.SitesPrefix,
                pageIndex, pageSize, sites.Count);

            var siteAdapter = (ISiteAdapter)ServiceProvider.GetService(typeof(ISiteAdapter));

            var selectedSites = sites.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            foreach (var site in selectedSites)
            {
                collection.AddItem(siteAdapter.ModelToResource(site));
            }
            return collection;
        }

        [Fact]
        public async Task GetByQueryWithoutAuthExpectUnauthorized()
        {
            // Arrange
            var sites = _db.GenerateSites(3);

            // Act
            Service.AuthToken = TokenProvider.EmptyToken;
            await Service.Get("", HttpStatusCode.Unauthorized);

            // Assert
        }

        [Fact]
        public async Task GetByQueryWithInvalidAuthExpectUnauthorized()
        {
            // Arrange
            var sites = _db.GenerateSites(3);

            // Act
            Service.AuthToken = TokenProvider.InvalidToken;
            await Service.Get("", HttpStatusCode.Unauthorized);

            // Assert
        }

        [Fact]
        public async Task GetByQueryWithAdminPermissionsExpectOK()
        {
            // Arrange
            var sites = _db.GenerateSites(3);

            // Act
            Service.AuthToken = TokenProvider.AdminToken;
            var returnedSites = await Service.Get("", HttpStatusCode.OK);

            // Assert
            returnedSites.ShouldBeEquivalentTo(ConvertToResourceCollection(sites));
        }



        [Fact]
        public async Task GetByQueryWithInvalidPageIndexExpectBadRequest()
        {
            // Arrange
            _db.GenerateSites(3);

            // Act
            await Service.Get("?pageIndex=aaa", HttpStatusCode.BadRequest);

            // Assert
            // nothing to do: parsing errors will be returned in the payload
        }

        [Fact]
        public async Task GetByQueryWithInvalidPageSizeExpectBadRequest()
        {
            // Arrange
            _db.GenerateSites(3);

            // Act
            await Service.Get("?pageIndex=1&pageSize=aa", HttpStatusCode.BadRequest);

            // Assert
            // nothing to do: parsing errors will be returned in the payload
        }

        [Fact]
        public async Task GetByQueryWithSingleSiteExpectOK()
        {
            // Arrange
            var sites = _db.GenerateSites(1);

            // Act
            var returnedSites = await Service.Get("", HttpStatusCode.OK);

            // Assert
            returnedSites.ShouldBeEquivalentTo(ConvertToResourceCollection(sites));
        }

        [Fact]
        public async Task GetByQueryMultipleSitesExpectOK()
        {
            // Arrange
            var sites = _db.GenerateSites(10);

            // Act
            var returnedSites = await Service.Get("", HttpStatusCode.OK);

            // Assert
            returnedSites.ShouldBeEquivalentTo(ConvertToResourceCollection(sites));
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
            var sites = _db.GenerateSites(3);

            // Act
            var returnedSites = await Service.Get();

            // Assert
            returnedSites.ShouldBeEquivalentTo(ConvertToResourceCollection(sites));
        }

        [Fact]
        public async Task GetWithPageSizeExpectOK()
        {
            // Arrange
            var sites = _db.GenerateSites(3);

            // Act
            var returnedSites = await Service.Get("?pageSize=2");

            // Assert
            returnedSites.ShouldBeEquivalentTo(ConvertToResourceCollection(sites, 1, 2));
        }

        [Fact]
        public async Task GetWithPageIndexAndPageSizeExpectOK()
        {
            // Arrange
            var sites = _db.GenerateSites(8);

            // Act
            var returnedSites = await Service.Get("?pageIndex=3&pageSize=2");

            // Assert
            returnedSites.ShouldBeEquivalentTo(ConvertToResourceCollection(sites, 3, 2));
        }

        [Fact]
        public async Task GetWithPageIndexExpectOK()
        {
            // Arrange
            var sites = _db.GenerateSites(30);

            // Act
            var returnedSites = await Service.Get("?pageIndex=2");

            // Assert
            returnedSites.ShouldBeEquivalentTo(ConvertToResourceCollection(sites, 2));
        }

    }
}