using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using SolarMonitor.Data.Repositories;
using Models = SolarMonitor.Data.Models;
using Resources = SolarMonitor.Data.Resources;

namespace SolarMonitorApi.ModelBinders
{
    public class DeviceModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new System.ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(Models.Device))
            {
                // return it via a BinderTypeModelBinder because DeviceModelBinder needs DI
                return new BinderTypeModelBinder(typeof(DeviceModelBinder));
            }
            else if (context.Metadata.ModelType == typeof(Resources.Device))
            {
                // return it via a BinderTypeModelBinder because ResourceSensorModelBinder needs DI
                return new BinderTypeModelBinder(typeof(ResourceDeviceModelBinder));
            }

            return null;
        }
    }
}
