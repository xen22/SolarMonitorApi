using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using SolarMonitor.Data.Repositories;
using Models = SolarMonitor.Data.Models;
using Resources = SolarMonitor.Data.Resources;

namespace SolarMonitorApi.ModelBinders
{
    public class SensorModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new System.ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(Models.Sensor))
            {
                // return it via a BinderTypeModelBinder because SensorModelBinder needs DI
                return new BinderTypeModelBinder(typeof(SensorModelBinder));
            }
            else if (context.Metadata.ModelType == typeof(Resources.Sensor))
            {
                // return it via a BinderTypeModelBinder because ResourceSensorModelBinder needs DI
                return new BinderTypeModelBinder(typeof(ResourceSensorModelBinder));
            }

            return null;
        }
    }
}
