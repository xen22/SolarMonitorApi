using System.Collections.Generic;
using SolarMonitor.Data.Models;

namespace SolarMonitor.Data.Repositories
{
    public interface ISensorTypeRepository
    {
        IEnumerable<string> SensorTypes();
        SensorType SensorType(int id);
        SensorType SensorTypeByName(string sensorName);
    }
}
