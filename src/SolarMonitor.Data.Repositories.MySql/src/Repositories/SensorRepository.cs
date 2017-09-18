using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using SolarMonitor.Data.Models;
using System.Collections.Generic;

namespace SolarMonitor.Data.Repositories.MySql
{
    public class SensorRepository : EntityRepository<Sensor>, ISensorRepository
    {
        public SensorRepository(ApplicationDbContext dbContext)
        : base(dbContext)
        {
        }

        // This is a helper function
        // Keep this private because it exposes IQueryable, otherwise this repo would be a leaky abstraction
        private IQueryable<Sensor> AllSensors()
        {
            return _entitiesContext.Set<Sensor>()
                .Include(s => s.Type)
                .Include(s => s.Site);
        }

        public Sensor Sensor(Guid guid)
        {
            IQueryable<Sensor> data = _entitiesContext.Set<Sensor>();
            return data
                .Include(s => s.Type)
                .Include(s => s.Site)
                .SingleOrDefault(s => s.Guid == guid);
        }


        public PaginatedList<Sensor> FindSensors(
            int pageIndex, int pageSize,
            IEnumerable<string> sensorTypes = null,
            IEnumerable<string> sensorNames = null,
            IEnumerable<string> siteNames = null)
        {
            var result = AllSensors();

            if (sensorTypes?.Count() > 0)
            {
                result = result.Where(s => sensorTypes.Contains(s.Type.Name));
            }
            if (sensorNames?.Count() > 0)
            {
                result = result.Where(s => sensorNames.Contains(s.Name));
            }
            if (siteNames?.Count() > 0)
            {
                result = result.Where(s => siteNames.Contains(s.SiteName));
            }

            return result.ToPaginatedList(pageIndex, pageSize);
        }

        public bool Delete(Guid guid)
        {
            var sensor = _entitiesContext.Set<Sensor>().SingleOrDefault(s => s.Guid == guid);
            if (sensor == null)
            {
                return false;
            }
            DeleteGraph(sensor);
            return true;
        }
    }
}

