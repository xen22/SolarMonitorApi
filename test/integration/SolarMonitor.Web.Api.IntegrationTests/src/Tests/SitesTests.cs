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
    //[Collection("Site tests")]
    public class SitesTests : IntegrationTestHelper, IDisposable
    {
        protected SiteService Service { get; private set; }
        protected ISiteAdapter SiteAdapter { get; private set; }
        public SitesTests()
        {
            Service = new SiteService(_client, TokenProvider);
            SiteAdapter = (ISiteAdapter)ServiceProvider.GetService(typeof(ISiteAdapter));
            System.Console.WriteLine("-------- Begin");
        }

        public virtual void Dispose()
        {
            System.Console.WriteLine("-------- End");
            Service.AuthToken = TokenProvider.UserToken;
            _db.RecreateDatabase();
        }

        protected Resources.Site ConvertToResource(Models.Site site)
        {
            return SiteAdapter.ModelToResource(site);
        }

        protected Models.Site ConvertToModel(Resources.Site s)
        {
            return SiteAdapter.ResourceToModel(s);
        }


    }
}

