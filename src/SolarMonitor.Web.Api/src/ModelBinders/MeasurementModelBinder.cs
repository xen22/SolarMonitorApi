// using System;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc.ModelBinding;
// using SolarMonitor.Data.Models;
// using SolarMonitor.Data.Repositories;
// using CommonTypes = SolarMonitor.Data.CommonTypes;

// namespace SolarMonitorApi.ModelBinders
// {
//     public class MeasurementModelBinder : IModelBinder
//     {
//         EntityModelBinderHelper<Measurement, CommonTypes.MeasurementType> _helper;
//         private readonly IMeasurementTypeRepository _measurementTypeRepository;
//         private readonly ISiteRepository _siteRepository;

//         public MeasurementModelBinder(
//             IMeasurementTypeRepository measurementTypeRepository,
//             ISiteRepository siteRepository)
//         {
//             _helper = new EntityModelBinderHelper<Measurement, CommonTypes.MeasurementType>();
//             _measurementTypeRepository = measurementTypeRepository ?? throw new ArgumentNullException(nameof(measurementTypeRepository));
//             _siteRepository = siteRepository ?? throw new ArgumentNullException(nameof(siteRepository));
//         }

//         public Task BindModelAsync(ModelBindingContext bindingContext)
//         {
//             if (bindingContext == null)
//             {
//                 throw new System.ArgumentNullException(nameof(bindingContext));
//             }

//             try
//             {
//                 var measurement = _helper.GetEntity(bindingContext.HttpContext.Request.Body);
//                 if (measurement == null)
//                 {
//                     return Task.CompletedTask;
//                 }

//                 // set the Type property based on TypeId because other parts of the code may depend on it
//                 //device.Type = new DeviceType { Id = (int)deviceType, Name = deviceType.ToString() };
//                 measurement.Type = _measurementTypeRepository.MeasurementType(measurement.TypeId);

//                 // also set the Site property: we need to lookup the Site from db based on ID
//                 measurement.Site = _siteRepository.Site(measurement.SiteId);

//                 // set the Guid if it's not been set already
//                 if (measurement.Guid == Guid.Empty)
//                 {
//                     measurement.Guid = Guid.NewGuid();
//                 }

//                 bindingContext.Result = ModelBindingResult.Success(measurement);
//                 return Task.CompletedTask;
//             }
//             catch (Exception)
//             {
//                 return Task.CompletedTask;
//             }

//         }

//     }

// }