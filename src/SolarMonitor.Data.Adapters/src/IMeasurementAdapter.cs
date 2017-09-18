using Resources = SolarMonitor.Data.Resources;
using Models = SolarMonitor.Data.Models;

namespace SolarMonitor.Data.Adapters
{
    public interface IMeasurementAdapter : IDataAdapter<Models.Measurement, Resources.Measurement>
    {
    }
}
