using System.Collections.Generic;

namespace SolarMonitorApi.Services
{
    public interface ISensorTypeService
    {
        IEnumerable<string> SensorTypes();

    }
}
