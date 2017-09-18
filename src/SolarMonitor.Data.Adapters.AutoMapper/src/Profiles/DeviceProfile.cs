using AutoMapper;
using Models = SolarMonitor.Data.Models;
using Resources = SolarMonitor.Data.Resources;

namespace SolarMonitor.Data.Adapters.AutoMapper
{
    public class DeviceProfile : Profile
    {
        public DeviceProfile()
        {
            // models => resources
            CreateMap<Models.Device, Resources.Device>()
                .ForMember(resource => resource.Type, conf => conf.MapFrom(model => model.Type.Name))
                .ForMember(resource => resource.SiteName, conf => conf.MapFrom(model => model.Site.Name))
                .AfterMap((model, resource) => resource.GenerateLinks());

            CreateMap<Models.ChargeController, Resources.ChargeController>()
                .IncludeBase<Models.Device, Resources.Device>();

            CreateMap<Models.Inverter, Resources.Inverter>()
                .IncludeBase<Models.Device, Resources.Device>();

            CreateMap<Models.SolarArray, Resources.SolarArray>()
                .IncludeBase<Models.Device, Resources.Device>();

            CreateMap<Models.BatteryBank, Resources.BatteryBank>()
                .IncludeBase<Models.Device, Resources.Device>();

            CreateMap<Models.WeatherStation, Resources.WeatherStation>()
                .IncludeBase<Models.Device, Resources.Device>();

            // resources => models
            CreateMap<Resources.Device, Models.Device>()
                .ForMember(dest => dest.Site, opt => opt.ResolveUsing<DeviceSiteMemberResolver>())
                .ForMember(dest => dest.Type, opt => opt.ResolveUsing<DeviceTypeMemberResolver>())
                .AfterMap((res, model) => model.SiteId = model.Site.Id)
                .AfterMap((res, model) => model.TypeId = model.Type.Id);

            CreateMap<Resources.ChargeController, Models.ChargeController>()
                .IncludeBase<Resources.Device, Models.Device>();

            CreateMap<Resources.Inverter, Models.Inverter>()
                .IncludeBase<Resources.Device, Models.Device>();

            CreateMap<Resources.SolarArray, Models.SolarArray>()
                .IncludeBase<Resources.Device, Models.Device>();

            CreateMap<Resources.BatteryBank, Models.BatteryBank>()
                .IncludeBase<Resources.Device, Models.Device>();

            CreateMap<Resources.WeatherStation, Models.WeatherStation>()
                .IncludeBase<Resources.Device, Models.Device>();

        }
    }


}