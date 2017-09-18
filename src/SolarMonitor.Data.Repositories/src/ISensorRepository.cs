using System.Linq;
using System;
using SolarMonitor.Data.Models;
using System.Collections.Generic;

namespace SolarMonitor.Data.Repositories
{
    public interface ISensorRepository : IEntityRepository<Sensor>
    {
        Sensor Sensor(Guid guid);
        PaginatedList<Sensor> FindSensors(
            int pageIndex,
            int pageSize,
            IEnumerable<string> sensorTypes = null,
            IEnumerable<string> sensorNames = null,
            IEnumerable<string> siteNames = null);

        bool Delete(Guid guid);
    }
}
