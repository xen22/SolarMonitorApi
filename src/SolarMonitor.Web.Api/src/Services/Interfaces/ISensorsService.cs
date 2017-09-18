using System;
using SolarMonitorApi.RequestQueries;
using Resources = SolarMonitor.Data.Resources;
using Models = SolarMonitor.Data.Models;

namespace SolarMonitorApi.Services
{
    public interface ISensorsService : IEntityService<Resources.Sensor, GetSensorsRequestQuery>
    {
        (string uri, Resources.Sensor) Create(Resources.Sensor device);
        bool Delete(Guid guid);
    }
}
