using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SolarMonitorApi.Helpers;
using SolarMonitorApi.RequestQueries;
using CommonTypes = SolarMonitor.Data.CommonTypes;

namespace SolarMonitorApi.Validators
{
    public class DeviceTypesValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string deviceTypes = (string)value;

            var deviceTypeList = ParamSplitter.SplitParam(deviceTypes);
            foreach (var deviceType in deviceTypeList)
            {
                if (!Enum.IsDefined(typeof(CommonTypes.DeviceType), deviceType) ||
                     (CommonTypes.DeviceType)Enum.Parse(typeof(CommonTypes.DeviceType), deviceType) == CommonTypes.DeviceType.Unset)
                {
                    return new ValidationResult($"Invalid device type {deviceType}");
                }
            }
            return ValidationResult.Success;
        }
    }
}