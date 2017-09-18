using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SolarMonitorApi.Helpers;
using SolarMonitorApi.RequestQueries;
using CommonTypes = SolarMonitor.Data.CommonTypes;

namespace SolarMonitorApi.Validators
{
    public class SensorTypesValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string sensorTypes = (string)value;

            var sensorTypeList = ParamSplitter.SplitParam(sensorTypes);
            foreach (var sensorType in sensorTypeList)
            {
                if (!Enum.IsDefined(typeof(CommonTypes.SensorType), sensorType) ||
                     (CommonTypes.SensorType)Enum.Parse(typeof(CommonTypes.SensorType), sensorType) == CommonTypes.SensorType.Unset)
                {
                    return new ValidationResult($"Invalid sensor type {sensorType}");
                }
            }
            return ValidationResult.Success;
        }
    }
}