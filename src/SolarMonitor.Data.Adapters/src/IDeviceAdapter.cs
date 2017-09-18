using Resources = SolarMonitor.Data.Resources;
using Models = SolarMonitor.Data.Models;

namespace SolarMonitor.Data.Adapters
{
    public interface IDeviceAdapter : IDataAdapter<Models.Device, Resources.Device>
    {
    }
}
