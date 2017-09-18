using System.Linq;
using SolarMonitor.Data.Models;

namespace SolarMonitor.Data.Repositories.MySql
{
    public class LoadTypeRepository : EntityRepository<LoadType>, ILoadTypeRepository
    {
        public LoadTypeRepository(ApplicationDbContext entitiesContext) : base(entitiesContext)
        {
        }

        public LoadType GetByType(string type)
        {
            return _entitiesContext.Set<LoadType>().Where(l => l.Type == type).SingleOrDefault();
        }
    }
}