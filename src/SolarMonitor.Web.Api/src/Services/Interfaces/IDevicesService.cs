using System;
using System.Collections.Generic;
using SolarMonitorApi.RequestQueries;
using Resources = SolarMonitor.Data.Resources;
using Models = SolarMonitor.Data.Models;

namespace SolarMonitorApi.Services
{
    public interface IDevicesService : IEntityService<Resources.Device, GetDevicesRequestQuery>
    {
        (string uri, Resources.Device) Create(Resources.Device device);
        bool Delete(Guid guid);
    }
}
