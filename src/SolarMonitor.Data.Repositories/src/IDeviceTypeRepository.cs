using System.Collections.Generic;
using SolarMonitor.Data.Models;

namespace SolarMonitor.Data.Repositories
{
    public interface IDeviceTypeRepository
    {
        IEnumerable<string> DeviceTypes();
        DeviceType DeviceType(int id);
        DeviceType DeviceTypeByName(string type);
    }
}
