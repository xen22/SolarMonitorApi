using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SolarMonitorApi.Validators
{
    class ModelStateValidator
    {
        public static void Validate(ModelStateDictionary m)
        {
            if (!m.IsValid)
            {
                var error = m.SelectMany(x => x.Value.Errors).First();
                if (error.ErrorMessage != null && error.ErrorMessage != String.Empty)
                {
                    throw new InvalidParameterException($"Parameter validation error: {error.ErrorMessage}");
                }
                else if (error.Exception?.Message != null)
                {
                    throw new InvalidParameterException(error.Exception.Message);
                }
                else
                {
                    throw new InvalidParameterException($"Unknown error while parsing modelState {m.ToString()}");
                }
            }
        }
    }
}