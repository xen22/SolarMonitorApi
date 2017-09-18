using Resources = SolarMonitor.Data.Resources;
using Models = SolarMonitor.Data.Models;
using AutoMapper;

namespace SolarMonitor.Data.Adapters.AutoMapper
{
    public class DeviceAdapter : GenericAdapter<Models.Device, Resources.Device>, IDeviceAdapter
    {
        public DeviceAdapter(IMapper mapper) : base(mapper)
        {
        }
    }
}