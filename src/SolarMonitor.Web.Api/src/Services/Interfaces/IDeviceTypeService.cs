using System.Collections.Generic;

namespace SolarMonitorApi.Services
{
    public interface IDeviceTypeService
    {
        IEnumerable<string> DeviceTypes();

    }

}