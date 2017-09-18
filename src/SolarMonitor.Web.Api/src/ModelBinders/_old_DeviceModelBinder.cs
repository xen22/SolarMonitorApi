using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SolarMonitor.Data.Models;
using SolarMonitor.Data.Repositories;
using CommonTypes = SolarMonitor.Data.CommonTypes;

namespace SolarMonitorApi.ModelBinders
{
    public class _old_DeviceModelBinder : IModelBinder
    {
        private readonly IDeviceTypeRepository _deviceTypeRepository;
        private readonly ISiteRepository _siteRepository;

        public _old_DeviceModelBinder(
            IDeviceTypeRepository deviceTypeRepository,
            ISiteRepository siteRepository)
        {
            _deviceTypeRepository = deviceTypeRepository ?? throw new ArgumentNullException(nameof(deviceTypeRepository));
            _siteRepository = siteRepository ?? throw new ArgumentNullException(nameof(siteRepository));
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new System.ArgumentNullException(nameof(bindingContext));
            }

            string jsonData = string.Empty;
            using (var sr = new StreamReader(bindingContext.HttpContext.Request.Body))
            {
                jsonData = sr.ReadToEnd();
            }

            if (string.IsNullOrEmpty(jsonData))
            {
                return Task.CompletedTask;
            }

            var deviceType = (CommonTypes.DeviceType)Convert.ToInt32(((JValue)JObject.Parse(jsonData)["typeId"]).Value);

            // Device device = null;
            // if (deviceType == CommonTypes.DeviceType.ChargeController)
            // {
            //     device = JsonConvert.DeserializeObject<ChargeController>(jsonData);
            // }

            var typeName = $"{typeof(Device).Namespace}.{deviceType}, {typeof(Device).GetTypeInfo().Assembly}";
            var typeName2 = typeof(Device).AssemblyQualifiedName;
            var devType = Type.GetType(typeName);
            var device = (Device)JsonConvert.DeserializeObject(jsonData, devType);

            // set the Type property based on TypeId because other parts of the code may depend on it
            //device.Type = new DeviceType { Id = (int)deviceType, Name = deviceType.ToString() };
            device.Type = _deviceTypeRepository.DeviceType((int)deviceType);

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
    }
}