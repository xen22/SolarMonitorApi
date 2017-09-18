using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SolarMonitor.Data.Models;

namespace SolarMonitor.Data.Repositories.MySql
{
    public class DeviceTypeRepository : EntityRepository<SolarMonitor.Data.Models.DeviceType>, IDeviceTypeRepository
    {
        public DeviceTypeRepository(ApplicationDbContext entitiesContext)
        : base(entitiesContext)
        { }

        public IEnumerable<string> DeviceTypes()
        {
            return GetAll().Select(d => d.Name);
        }

        public DeviceType DeviceType(int id)
        {
            return GetSingle(id);
        }

        public DeviceType DeviceTypeByName(string type)
        {
            return GetAll().Where(t => t.Name == type).SingleOrDefault();
        }

    }
}