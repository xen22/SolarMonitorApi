using AutoMapper;
using SolarMonitor.Data.Repositories;
using Models = SolarMonitor.Data.Models;
using Resources = SolarMonitor.Data.Resources;

namespace SolarMonitor.Data.Adapters.AutoMapper
{
    public class SensorTypeMemberResolver : IValueResolver<Resources.Sensor, Models.Sensor, Models.SensorType>
    {
        private ISensorTypeRepository _SensorTypeRepository;
        public SensorTypeMemberResolver(ISensorTypeRepository SensorTypeRepository)
        {
            _SensorTypeRepository = SensorTypeRepository ?? throw new System.ArgumentNullException(nameof(SensorTypeRepository));
        }

        public Models.SensorType Resolve(Resources.Sensor source, Models.Sensor destination,
            Models.SensorType destMember, ResolutionContext context)
        {
            return _SensorTypeRepository.SensorTypeByName(source.Type);
        }
    }
}
