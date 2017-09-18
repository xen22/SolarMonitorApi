using System;
using System.Collections.Generic;
using System.Linq;
using SolarMonitor.Data.Models;

namespace SolarMonitor.Data.Repositories
{
    public interface IDeviceRepository : IEntityRepository<Device>
    {
        Device Device(Guid guid);
        PaginatedList<Device> FindDevices(
            int pageIndex,
            int pageSize,
            IEnumerable<string> deviceTypes = null,
            IEnumerable<string> deviceNames = null,
            IEnumerable<string> siteNames = null);

        bool Delete(Guid guid);
    }
}
