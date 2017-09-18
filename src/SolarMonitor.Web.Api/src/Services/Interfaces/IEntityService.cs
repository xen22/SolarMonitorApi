using System;
using SolarMonitorApi.RequestQueries;
using Resources = SolarMonitor.Data.Resources;

namespace SolarMonitorApi.Services
{
    public interface IEntityService<EntityT, QueryT>
    {
        Resources.CollectionResource<EntityT> Get(QueryT query);
        EntityT GetSingle(Guid guid);
    }
}
