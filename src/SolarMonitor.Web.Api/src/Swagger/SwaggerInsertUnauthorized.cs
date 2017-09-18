using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SolarMonitorApi
{
    public class SwaggerInsertUnauthorized : IOperationFilter
    {
        void IOperationFilter.Apply(Operation operation, OperationFilterContext context)
        {
            if(!operation.Responses.ContainsKey("401"))
            {
                operation.Responses.Add("401", new Response { Description = "Unauthorized: token or credentials missing or invalid" });
            }
        }
    }
}
