using System.Net;
using System.Threading.Tasks;
using Resources = SolarMonitor.Data.Resources;

namespace SolarMonitor.Api.IntegrationTests.Services
{
    public interface IApiService<TModel, TResource, TId>
        where TResource : class
    {
        Task<TResource> GetSingle(
            TId id,
            HttpStatusCode expectedStatusCode = HttpStatusCode.OK);
        Task<Resources.CollectionResource<TResource>> Get(
            string query = "",
            HttpStatusCode expectedStatusCode = HttpStatusCode.OK);
        Task<TResource> Create(
            TResource m,
            HttpStatusCode expectedStatusCode = HttpStatusCode.Created);
        Task Delete(
            TId id,
            HttpStatusCode expectedStatusCode = HttpStatusCode.NoContent);
    }
}
