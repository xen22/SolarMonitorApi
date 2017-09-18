using System;
using SolarMonitor.Data.Models;
using SolarMonitorApi.RequestQueries;
using Resources = SolarMonitor.Data.Resources;

namespace SolarMonitorApi.Services
{
    public interface ISiteService : IEntityService<Resources.Site, GetSitesRequestQuery>
    {
        (string uri, Resources.Site) Create(Resources.Site site);
        bool Delete(Guid guid);
    }
}
