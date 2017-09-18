using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using SolarMonitor.Data.Models;
using SolarMonitor.Data.Repositories;
using CommonTypes = SolarMonitor.Data.CommonTypes;

namespace SolarMonitorApi.ModelBinders
{
    public class SensorModelBinder : IModelBinder
    {
        EntityModelBinderHelper<Sensor, CommonTypes.SensorType> _helper;
        private readonly ISensorTypeRepository _sensorTypeRepository;
        private readonly ISiteRepository _siteRepository;

        public SensorModelBinder(
            ISensorTypeRepository sensorTypeRepository,
            ISiteRepository siteRepository)
        {
            _helper = new EntityModelBinderHelper<Sensor, CommonTypes.SensorType>();
            _sensorTypeRepository = sensorTypeRepository ?? throw new ArgumentNullException(nameof(sensorTypeRepository));
            _siteRepository = siteRepository ?? throw new ArgumentNullException(nameof(siteRepository));
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new System.ArgumentNullException(nameof(bindingContext));
            }

            try
            {
                var sensor = _helper.GetEntity(bindingContext.HttpContext.Request.Body);
                if (sensor == null)
                {
                    bindingContext.ModelState.AddModelError("Sensor", "Unable to parse JSON data from message body.");
                    return Task.CompletedTask;
                }

                // set the Type property based on TypeId because other parts of the code may depend on it
                //device.Type = new DeviceType { Id = (int)deviceType, Name = deviceType.ToString() };
                sensor.Type = _sensorTypeRepository.SensorType(sensor.TypeId);

                // also set the Site property: we need to lookup the Site from db based on ID
                sensor.Site = _siteRepository.Site(sensor.SiteId);

                // set the Guid if it's not been set already
                if (sensor.Guid == Guid.Empty)
                {
                    sensor.Guid = Guid.NewGuid();
                }

                bindingContext.Result = ModelBindingResult.Success(sensor);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.AddModelError("Sensor", $"Parsing error: {ex.Message}");
                return Task.CompletedTask;
            }

        }

    }

}