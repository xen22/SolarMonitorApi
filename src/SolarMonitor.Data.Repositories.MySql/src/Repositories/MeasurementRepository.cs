using System.Linq;
using Microsoft.EntityFrameworkCore;
using SolarMonitor.Data.Models;

namespace SolarMonitor.Data.Repositories.MySql
{
    public class MeasurementRepository : IMeasurementRepository
    {
        ApplicationDbContext _dbContext;
        public MeasurementRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public PaginatedList<Measurement> FindMeasurements(
            int pageIndex,
            int pageSize)
        {
            return _dbContext.Set<Measurement>()
                .Include("Sensor.Type")
                .Include("Sensor.Name")
                .ToPaginatedList(pageIndex, pageSize);
        }
    }

}