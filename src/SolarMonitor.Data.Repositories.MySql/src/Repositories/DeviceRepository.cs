using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SolarMonitor.Data.Models;

namespace SolarMonitor.Data.Repositories.MySql
{
    public class DeviceRepository : EntityRepository<Device>, IDeviceRepository
    {
        public DeviceRepository(ApplicationDbContext dbContext)
        : base(dbContext)
        {
        }

        // This is a helper function
        // Keep this private because it exposes IQueryable, otherwise this repo would be a leaky abstraction
        private IQueryable<Device> AllDevices()
        {
            return _entitiesContext.Set<Device>()
                .Include(d => d.Type)
                .Include(s => s.Site);
        }

        public Device Device(Guid guid)
        {
            IQueryable<Device> data = _entitiesContext.Set<Device>();
            return data
                .Include(d => d.Type)
                .Include(d => d.Site)
                .SingleOrDefault(d => d.Guid == guid);
        }


        public PaginatedList<Device> FindDevices(
            int pageIndex, int pageSize,
            IEnumerable<string> deviceTypes = null,
            IEnumerable<string> deviceNames = null,
            IEnumerable<string> siteNames = null)
        {
            var result = AllDevices();

            if (deviceTypes?.Count() > 0)
            {
                result = result.Where(d => deviceTypes.Contains(d.Type.Name));
            }
            if (deviceNames?.Count() > 0)
            {
                result = result.Where(d => deviceNames.Contains(d.Name));
            }
            if (siteNames?.Count() > 0)
            {
                result = result.Where(d => siteNames.Contains(d.SiteName));
            }

            return result.ToPaginatedList(pageIndex, pageSize);
        }

        public bool Delete(Guid guid)
        {
            var device = _entitiesContext.Set<Device>().SingleOrDefault(s => s.Guid == guid);
            if (device == null)
            {
                return false;
            }

            // now look for all Sensors that have this device as their parent
            var sensors = _entitiesContext.Set<Sensor>().Where(s => s.Device == guid);
            foreach (var sensor in sensors)
            {
                EntityEntry dbEntityEntry = _entitiesContext.Entry<Sensor>(sensor);
                dbEntityEntry.State = EntityState.Deleted;
            }

            // delete the device and associated entities
            DeleteGraph(device);

            return true;
        }

    }

}