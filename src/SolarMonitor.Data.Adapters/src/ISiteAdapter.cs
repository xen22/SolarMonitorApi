using Resources = SolarMonitor.Data.Resources;
using Models = SolarMonitor.Data.Models;

namespace SolarMonitor.Data.Adapters
{

    public interface ISiteAdapter : IDataAdapter<Models.Site, Resources.Site>
    {
    }
}