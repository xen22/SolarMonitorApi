using AutoMapper;
using Models = SolarMonitor.Data.Models;
using Resources = SolarMonitor.Data.Resources;

namespace SolarMonitor.Data.Adapters.AutoMapper
{
    public class SensorProfile : Profile
    {
        public SensorProfile()
        {
            // models => resources
            CreateMap<Models.Sensor, Resources.Sensor>()
                .ForMember(resource => resource.Type, conf => conf.MapFrom(model => model.Type.Name))
                .ForMember(resource => resource.Site, conf => conf.MapFrom(model => model.Site.Name))
                .AfterMap((model, resource) => resource.GenerateLinks());

            CreateMap<Models.Shunt, Resources.Shunt>()
                .IncludeBase<Models.Sensor, Resources.Sensor>();

            CreateMap<Models.CurrentSensor, Resources.CurrentSensor>()
                .IncludeBase<Models.Sensor, Resources.Sensor>();

            CreateMap<Models.BatterySocSensor, Resources.BatterySocSensor>()
                .IncludeBase<Models.Sensor, Resources.Sensor>();

            CreateMap<Models.BatteryChargeStageSensor, Resources.BatteryChargeStageSensor>()
                .IncludeBase<Models.Sensor, Resources.Sensor>();

            CreateMap<Models.ChargeControllerLoadOutput, Resources.ChargeControllerLoadOutput>()
                .IncludeBase<Models.Sensor, Resources.Sensor>();

            CreateMap<Models.BatteryStatsSensor, Resources.BatteryStatsSensor>()
                .IncludeBase<Models.Sensor, Resources.Sensor>();

            CreateMap<Models.EnergyStatsSensor, Resources.EnergyStatsSensor>()
                .IncludeBase<Models.Sensor, Resources.Sensor>();

            CreateMap<Models.TemperatureSensor, Resources.TemperatureSensor>()
                .IncludeBase<Models.Sensor, Resources.Sensor>();

            CreateMap<Models.HumiditySensor, Resources.HumiditySensor>()
                .IncludeBase<Models.Sensor, Resources.Sensor>();

            CreateMap<Models.BarometricPressureSensor, Resources.BarometricPressureSensor>()
                .IncludeBase<Models.Sensor, Resources.Sensor>();

            CreateMap<Models.WindSensor, Resources.WindSensor>()
                .IncludeBase<Models.Sensor, Resources.Sensor>();

            // resources => models
            CreateMap<Resources.Sensor, Models.Sensor>()
                .ForMember(dest => dest.Site, opt => opt.ResolveUsing<SensorSiteMemberResolver>())
                .ForMember(dest => dest.SiteId, opt => opt.ResolveUsing<SensorSiteIdMemberResolver>())
                // .AfterMap((res, model) => model.SiteId = model.Site.Id)
                // .AfterMap((res, model) => model.TypeId = model.Type.Id);
                .ForMember(dest => dest.Type, opt => opt.ResolveUsing<SensorTypeMemberResolver>())
                .ForMember(dest => dest.TypeId, opt => opt.ResolveUsing<SensorTypeIdMemberResolver>());

            CreateMap<Resources.Shunt, Models.Shunt>()
                .IncludeBase<Resources.Sensor, Models.Sensor>();

            CreateMap<Resources.CurrentSensor, Models.CurrentSensor>()
                .IncludeBase<Resources.Sensor, Models.Sensor>();

            CreateMap<Resources.BatterySocSensor, Models.BatterySocSensor>()
                .IncludeBase<Resources.Sensor, Models.Sensor>();

            CreateMap<Resources.BatteryChargeStageSensor, Models.BatteryChargeStageSensor>()
                .IncludeBase<Resources.Sensor, Models.Sensor>();

            CreateMap<Resources.ChargeControllerLoadOutput, Models.ChargeControllerLoadOutput>()
                .IncludeBase<Resources.Sensor, Models.Sensor>();

            CreateMap<Resources.BatteryStatsSensor, Models.BatteryStatsSensor>()
                .IncludeBase<Resources.Sensor, Models.Sensor>();

            CreateMap<Resources.EnergyStatsSensor, Models.EnergyStatsSensor>()
                .IncludeBase<Resources.Sensor, Models.Sensor>();

            CreateMap<Resources.TemperatureSensor, Models.TemperatureSensor>()
                .IncludeBase<Resources.Sensor, Models.Sensor>();

            CreateMap<Resources.HumiditySensor, Models.HumiditySensor>()
                .IncludeBase<Resources.Sensor, Models.Sensor>();

            CreateMap<Resources.BarometricPressureSensor, Models.BarometricPressureSensor>()
                .IncludeBase<Resources.Sensor, Models.Sensor>();

            CreateMap<Resources.WindSensor, Models.WindSensor>()
                .IncludeBase<Resources.Sensor, Models.Sensor>();

        }
    }


}