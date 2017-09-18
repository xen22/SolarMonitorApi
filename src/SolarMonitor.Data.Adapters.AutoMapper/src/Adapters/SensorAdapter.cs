using Resources = SolarMonitor.Data.Resources;
using Models = SolarMonitor.Data.Models;
using AutoMapper;

namespace SolarMonitor.Data.Adapters.AutoMapper
{
    public class SensorAdapter : GenericAdapter<Models.Sensor, Resources.Sensor>, ISensorAdapter
    {
        public SensorAdapter(IMapper mapper) : base(mapper)
        {
        }
    }
}