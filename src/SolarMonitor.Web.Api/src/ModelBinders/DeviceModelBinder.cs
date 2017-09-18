using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SolarMonitor.Data.Models;
using SolarMonitor.Data.Repositories;
using CommonTypes = SolarMonitor.Data.CommonTypes;

namespace SolarMonitorApi.ModelBinders
{
    public class DeviceModelBinder : IModelBinder
    {
        EntityModelBinderHelper<Device, CommonTypes.DeviceType> _helper;
        private readonly IDeviceTypeRepository _deviceTypeRepository;
        private readonly ISiteRepository _siteRepository;

        public DeviceModelBinder(
            IDeviceTypeRepository deviceTypeRepository,
            ISiteRepository siteRepository)
        {
            _helper = new EntityModelBinderHelper<Device, CommonTypes.DeviceType>();
            _deviceTypeRepository = deviceTypeRepository ?? throw new ArgumentNullException(nameof(deviceTypeRepository));
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
                var device = _helper.GetEntity(bindingContext.HttpContext.Request.Body);
                if (device == null)
                {
                    return Task.CompletedTask;
                }

                // set the Type property based on TypeId because other parts of the code may depend on it
                //device.Type = new DeviceType { Id = (int)deviceType, Name = deviceType.ToString() };
                device.Type = _deviceTypeRepository.DeviceType(device.TypeId);

                // also set the Site property: we need to lookup the Site from db based on ID
                device.Site = _siteRepository.Site(device.SiteId);

                // set the Guid if it's not been set already
                if (device.Guid == Guid.Empty)
                {
                    device.Guid = Guid.NewGuid();
                }

                bindingContext.Result = ModelBindingResult.Success(device);
                return Task.CompletedTask;
            }
            catch (Exception)
            {
                return Task.CompletedTask;
            }

        }

    }

}