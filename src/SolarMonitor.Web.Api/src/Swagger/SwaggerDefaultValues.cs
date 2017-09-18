using System.Collections.Generic;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SolarMonitorApi
{
    /// <summary>
    /// Inserts default values into the Swagger UI edit boxes for controller parameters.
    /// </summary>
    public class SwaggerDefaultValues : IOperationFilter
    {
        void IOperationFilter.Apply(Operation operation, OperationFilterContext context)
        {
            try
            {
                var requiredParams = new List<string>();
                //var defaultValues = new Dictionary<string, string>();

                foreach (var p in context.ApiDescription.ParameterDescriptions)
                {
                    if (p.ModelMetadata.IsRequired)
                    {
                        requiredParams.Add(p.Name);
                    }
                    //defaultValues.Add(p.Name, p.ModelMetadata.)
                }

                if (operation?.Parameters != null)
                {

                    foreach (var param in operation.Parameters)
                    {
                        // if(defaultValues.ContainsKey(param.Name))
                        // {
                        //     param.Description += $"(default: {defaultValues[param.Name]})";
                        // }
                        param.Required = requiredParams.Contains(param.Name);
                    }

                    // This worked in ASP.NET 5, but in Core, ParameterDescriptor's DefaultValue property does not exist:
                    // var parameterValuePairs = 
                    //     context.ApiDescription.ActionDescriptor.Parameters
                    //         .Where(parameter => parameter.DefaultValue != null)
                    //         .ToDictionary(parameter => parameter.ParameterName, parameter => parameter.DefaultValue);

                    // foreach (var param in operation.Parameters)
                    // {
                    //     object defaultValue;
                    //     if (parameterValuePairs.TryGetValue(param.Name, out defaultValue))
                    //         param.@default = defaultValue;
                    // }
                }
            }
            catch { }
        }
    }
}