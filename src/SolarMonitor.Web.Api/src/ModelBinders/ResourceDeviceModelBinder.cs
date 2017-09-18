using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using SolarMonitor.Data.Resources;
using SolarMonitor.Data.Repositories;
using CommonTypes = SolarMonitor.Data.CommonTypes;

namespace SolarMonitorApi.ModelBinders
{
    public class ResourceDeviceModelBinder : IModelBinder
    {
        private readonly ResourceModelBinderHelper<Device, CommonTypes.DeviceType> _helper;

        public ResourceDeviceModelBinder()
        {
            _helper = new ResourceModelBinderHelper<Device, CommonTypes.DeviceType>();
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new System.ArgumentNullException(nameof(bindingContext));
            }

            try
            {
                var Device = _helper.GetEntity(bindingContext.HttpContext.Request.Body);
                if (Device == null)
                {
                    bindingContext.ModelState.AddModelError("Device", "Unable to parse JSON data from message body.");
                    return Task.CompletedTask;
                }

                if (Device.Guid == Guid.Empty)
                {
                    Device.Guid = Guid.NewGuid();
                }

                bindingContext.Result = ModelBindingResult.Success(Device);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.AddModelError("Device", $"Parsing error: {ex.Message}");
                return Task.CompletedTask;
            }

        }

    }

}