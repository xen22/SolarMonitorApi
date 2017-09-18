using System.Collections.Generic;
using System.Linq;
using SolarMonitor.Data.Models;

namespace SolarMonitor.Data.Repositories.MySql
{
    public class SensorTypeRepository : EntityRepository<SolarMonitor.Data.Models.SensorType>, ISensorTypeRepository
    {
        public SensorTypeRepository(ApplicationDbContext entitiesContext)
        : base(entitiesContext)
        { }

        public IEnumerable<string> SensorTypes()
        {
            return GetAll().Select(s => s.Name);
        }

        public SensorType SensorType(int id)
        {
            return GetSingle(id);
        }

        public SensorType SensorTypeByName(string sensorName)
        {
            return GetAll().Where(t => t.Name == sensorName).SingleOrDefault();
        }
    }
}