using System;
using System.Linq;
using SolarMonitor.Data.Models;

namespace SolarMonitor.Data.Repositories
{
    public interface IMeasurementRepository
    {
        // TODO: add other query parameters
        PaginatedList<Measurement> FindMeasurements(
            int pageIndex,
            int pageSize);

    }
}
