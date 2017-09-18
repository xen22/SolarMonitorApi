using Resources = SolarMonitor.Data.Resources;
using Models = SolarMonitor.Data.Models;
using AutoMapper;

namespace SolarMonitor.Data.Adapters.AutoMapper
{
    public class MeasurementAdapter : GenericAdapter<Models.Measurement, Resources.Measurement>, IMeasurementAdapter
    {
        public MeasurementAdapter(IMapper mapper) : base(mapper)
        {
        }
    }
}