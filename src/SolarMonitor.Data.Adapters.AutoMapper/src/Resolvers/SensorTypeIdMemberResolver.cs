using AutoMapper;
using SolarMonitor.Data.Repositories;
using Models = SolarMonitor.Data.Models;
using Resources = SolarMonitor.Data.Resources;

namespace SolarMonitor.Data.Adapters.AutoMapper
{
    public class SensorTypeIdMemberResolver : IValueResolver<Resources.Sensor, Models.Sensor, int>
    {
        private ISensorTypeRepository _SensorTypeRepository;
        public SensorTypeIdMemberResolver(ISensorTypeRepository SensorTypeRepository)
        {
            _SensorTypeRepository = SensorTypeRepository ?? throw new System.ArgumentNullException(nameof(SensorTypeRepository));
        }

        public int Resolve(Resources.Sensor source, Models.Sensor destination,
            int destMember, ResolutionContext context)
        {
            return _SensorTypeRepository.SensorTypeByName(source.Type).Id;
        }
    }
}
