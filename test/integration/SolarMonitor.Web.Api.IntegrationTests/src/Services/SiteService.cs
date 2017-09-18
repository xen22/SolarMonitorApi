using System;
using System.Net.Http;
using Models = SolarMonitor.Data.Models;
using Resources = SolarMonitor.Data.Resources;

namespace SolarMonitor.Api.IntegrationTests.Services
{
    public class SiteService : ApiService<Models.Site, Resources.Site, Guid>
    {
        public SiteService(HttpClient client, TokenProvider tokenProvider) : base(client, "sites", tokenProvider)
        {
        }
    }

}