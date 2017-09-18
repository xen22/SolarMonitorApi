using AutoMapper;
using SolarMonitor.Data.Repositories;
using Models = SolarMonitor.Data.Models;
using Resources = SolarMonitor.Data.Resources;

namespace SolarMonitor.Data.Adapters.AutoMapper
{
    public class DeviceTypeMemberResolver : IValueResolver<Resources.Device, Models.Device, Models.DeviceType>
    {
        private IDeviceTypeRepository _deviceTypeRepository;
        public DeviceTypeMemberResolver(IDeviceTypeRepository deviceTypeRepository)
        {
            _deviceTypeRepository = deviceTypeRepository ?? throw new System.ArgumentNullException(nameof(deviceTypeRepository));
        }

        public Models.DeviceType Resolve(Resources.Device source, Models.Device destination,
            Models.DeviceType destMember, ResolutionContext context)
        {
            return _deviceTypeRepository.DeviceTypeByName(source.Type);
        }
    }
}
