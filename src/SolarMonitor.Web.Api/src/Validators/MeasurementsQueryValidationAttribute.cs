using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SolarMonitorApi.RequestQueries;
using Models = SolarMonitor.Data.Models;
using CommonTypes = SolarMonitor.Data.CommonTypes;
using System.ComponentModel.DataAnnotations;

namespace SolarMonitorApi.Validators
{
    class MeasurementsQueryValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            GetMeasurementsRequestQuery query = (GetMeasurementsRequestQuery)validationContext.ObjectInstance;

            if (((query.SensorType == null) || (query.SensorType == CommonTypes.SensorType.Unset)) &&
               (query.SensorGuid == null))
            {
                return new ValidationResult("Either SensorType or SensorGuid needs to be passed in.");
            }

            if ((query.SensorType != null) &&
                 !Models.Sensor.SupportedMeasurements(query.SensorType).Any(mt => mt == query.Type))
            {
                return new ValidationResult($"Sensor type {query.SensorType.ToString()} does not support measurement type {query.Type}");
            }
            return ValidationResult.Success;
        }
    }

}