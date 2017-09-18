using AutoMapper;
using Models = SolarMonitor.Data.Models;
using Resources = SolarMonitor.Data.Resources;

namespace SolarMonitor.Data.Adapters.AutoMapper
{
    public class MeasurementProfile : Profile
    {
        public MeasurementProfile()
        {
            // models => resources
            CreateMap<Models.Measurement, Resources.Measurement>()
                .ForMember(resource => resource.Type, conf => conf.MapFrom(model => model.Sensor.Type))
                .ForMember(resource => resource.SensorGuid, conf => conf.MapFrom(model => model.Sensor.Guid))
                .AfterMap((model, resource) => resource.GenerateLinks());

            CreateMap<Models.ShuntMeasurement, Resources.ShuntMeasurement>()
                .IncludeBase<Models.Measurement, Resources.Measurement>();

            CreateMap<Models.CurrentSensorMeasurement, Resources.CurrentSensorMeasurement>()
                .IncludeBase<Models.Measurement, Resources.Measurement>();

            CreateMap<Models.BatterySocMeasurement, Resources.BatterySocMeasurement>()
                .IncludeBase<Models.Measurement, Resources.Measurement>();

            CreateMap<Models.BatteryChargeStageMeasurement, Resources.BatteryChargeStageMeasurement>()
                .IncludeBase<Models.Measurement, Resources.Measurement>();

            CreateMap<Models.ChargeControllerLoadOutputMeasurement, Resources.ChargeControllerLoadOutputMeasurement>()
                .IncludeBase<Models.Measurement, Resources.Measurement>();

            CreateMap<Models.BatteryStatsMeasurement, Resources.BatteryStatsMeasurement>()
                .IncludeBase<Models.Measurement, Resources.Measurement>();

            CreateMap<Models.EnergyStatsMeasurement, Resources.EnergyStatsMeasurement>()
                .IncludeBase<Models.Measurement, Resources.Measurement>();

            CreateMap<Models.TemperatureMeasurement, Resources.TemperatureMeasurement>()
                .IncludeBase<Models.Measurement, Resources.Measurement>();

            CreateMap<Models.HumidityMeasurement, Resources.HumidityMeasurement>()
                .IncludeBase<Models.Measurement, Resources.Measurement>();

            CreateMap<Models.BarometricPressureMeasurement, Resources.BarometricPressureMeasurement>()
                .IncludeBase<Models.Measurement, Resources.Measurement>();

            CreateMap<Models.WindMeasurement, Resources.WindMeasurement>()
                .IncludeBase<Models.Measurement, Resources.Measurement>();

            // resources => models
            CreateMap<Resources.Measurement, Models.Measurement>()
                .ForMember(dest => dest.Sensor, opt => opt.ResolveUsing<MeasurementSensorMemberResolver>());

            CreateMap<Resources.ShuntMeasurement, Models.ShuntMeasurement>()
                .IncludeBase<Resources.Measurement, Models.Measurement>();

            CreateMap<Resources.CurrentSensorMeasurement, Models.CurrentSensorMeasurement>()
                .IncludeBase<Resources.Measurement, Models.Measurement>();

            CreateMap<Resources.BatterySocMeasurement, Models.BatterySocMeasurement>()
                .IncludeBase<Resources.Measurement, Models.Measurement>();

            CreateMap<Resources.BatteryChargeStageMeasurement, Models.BatteryChargeStageMeasurement>()
                .IncludeBase<Resources.Measurement, Models.Measurement>();

            CreateMap<Resources.ChargeControllerLoadOutputMeasurement, Models.ChargeControllerLoadOutputMeasurement>()
                .IncludeBase<Resources.Measurement, Models.Measurement>();

            CreateMap<Resources.BatteryStatsMeasurement, Models.BatteryStatsMeasurement>()
                .IncludeBase<Resources.Measurement, Models.Measurement>();

            CreateMap<Resources.EnergyStatsMeasurement, Models.EnergyStatsMeasurement>()
                .IncludeBase<Resources.Measurement, Models.Measurement>();

            CreateMap<Resources.TemperatureMeasurement, Models.TemperatureMeasurement>()
                .IncludeBase<Resources.Measurement, Models.Measurement>();

            CreateMap<Resources.HumidityMeasurement, Models.HumidityMeasurement>()
                .IncludeBase<Resources.Measurement, Models.Measurement>();

            CreateMap<Resources.BarometricPressureMeasurement, Models.BarometricPressureMeasurement>()
                .IncludeBase<Resources.Measurement, Models.Measurement>();

            CreateMap<Resources.WindMeasurement, Models.WindMeasurement>()
                .IncludeBase<Resources.Measurement, Models.Measurement>();

        }
    }


}