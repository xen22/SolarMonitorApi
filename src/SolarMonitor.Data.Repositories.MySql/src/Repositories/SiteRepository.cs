using System;
using System.Linq;
using SolarMonitor.Data.Models;

namespace SolarMonitor.Data.Repositories.MySql
{
    public class SiteRepository : EntityRepository<SolarMonitor.Data.Models.Site>, ISiteRepository
    {
        public SiteRepository(ApplicationDbContext entitiesContext)
        : base(entitiesContext)
        { }

        public Site Site(int id)
        {
            return GetSingle(id);
        }

        public Site Site(Guid guid)
        {
            return GetAll().Where(s => s.Guid == guid).SingleOrDefault();
        }

        public Site SiteByName(string name)
        {
            return GetAll().Where(s => s.Name == name).SingleOrDefault();
        }

        public PaginatedList<Site> Sites(int pageIndex, int pageSize)
        {
            return GetAll().ToPaginatedList(pageIndex, pageSize);
        }

        public bool Delete(Guid guid)
        {
            var site = _entitiesContext.Set<Site>().SingleOrDefault(s => s.Guid == guid);
            if (site == null)
            {
                return false;
            }
            DeleteGraph(site);
            return true;
        }
    }
}