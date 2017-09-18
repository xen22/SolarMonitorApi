using Resources = SolarMonitor.Data.Resources;
using Models = SolarMonitor.Data.Models;

namespace SolarMonitor.Data.Adapters
{
    public interface ISensorAdapter : IDataAdapter<Models.Sensor, Resources.Sensor>
    {
    }
}
