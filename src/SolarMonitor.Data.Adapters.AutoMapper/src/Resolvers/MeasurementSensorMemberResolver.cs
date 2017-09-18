using AutoMapper;
using SolarMonitor.Data.Repositories;
using Models = SolarMonitor.Data.Models;
using Resources = SolarMonitor.Data.Resources;

namespace SolarMonitor.Data.Adapters.AutoMapper
{
    public class MeasurementSensorMemberResolver : IValueResolver<Resources.Measurement, Models.Measurement, Models.Sensor>
    {
        private ISensorRepository _sensorRepository;
        public MeasurementSensorMemberResolver(ISensorRepository sensorRepository)
        {
            _sensorRepository = sensorRepository ?? throw new System.ArgumentNullException(nameof(sensorRepository));
        }

        public Models.Sensor Resolve(Resources.Measurement source, Models.Measurement destination,
            Models.Sensor destMember, ResolutionContext context)
        {
            return _sensorRepository.Sensor(source.SensorGuid);
        }
    }
}
