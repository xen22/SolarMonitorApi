using System;
using SolarMonitor.Data.Models;

namespace SolarMonitor.Data.Repositories
{
    public interface ISiteRepository : IEntityRepository<Site>
    {
        Site Site(int id);
        Site Site(Guid guid);
        Site SiteByName(string name);
        PaginatedList<Site> Sites(int pageIndex, int pageSize);
        bool Delete(Guid guid);
    }
}
