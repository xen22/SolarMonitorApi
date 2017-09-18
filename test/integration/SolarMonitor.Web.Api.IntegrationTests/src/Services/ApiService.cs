using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using Resources = SolarMonitor.Data.Resources;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json.Serialization;

namespace SolarMonitor.Api.IntegrationTests.Services
{
    public class ApiService<TModel, TResource, TId> : ApiServiceHelper, IApiService<TModel, TResource, TId>
        where TResource : class
    {
        private readonly string _name;
        public ApiService(HttpClient client, string name, TokenProvider tokenProvider) : base(client, tokenProvider)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Invalid name for API service", nameof(name));
            }

            _name = name;
        }

        public async Task<TResource> GetSingle(
            TId guid,
            HttpStatusCode expectedStatusCode = HttpStatusCode.NoContent)
        {
            return await GetSingle(guid.ToString(), expectedStatusCode);
        }

        public async Task<TResource> GetSingle(
            string guid,
            HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
        {
            var reqUri = $"{_baseUrl}/api/{_name}/{guid}";
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(reqUri),
                Method = HttpMethod.Get,
            };
            //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            AddAuthTokenHeader(request.Headers, AuthToken);

            var response = await _client.SendAsync(request);
            Assert.Equal(expectedStatusCode, response.StatusCode);
            string data = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(data) ||
                (response.StatusCode == HttpStatusCode.BadRequest))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<TResource>(data);
        }

        public async Task<Resources.CollectionResource<TResource>> Get(
            string queryString = "",
            HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
        {
            var reqUri = $"{_baseUrl}/api/{_name}{queryString}";
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(reqUri),
                Method = HttpMethod.Get,
            };
            //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            AddAuthTokenHeader(request.Headers, AuthToken);

            var response = await _client.SendAsync(request);
            Assert.Equal(expectedStatusCode, response.StatusCode);
            string data = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(data) || (response.StatusCode == HttpStatusCode.BadRequest))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<Resources.CollectionResource<TResource>>(data);
        }

        public virtual async Task<TResource> Create(
            TResource newEntity,
            HttpStatusCode expectedStatusCode = HttpStatusCode.Created)
        {
            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            return await Create(JsonConvert.SerializeObject(newEntity, Formatting.Indented, jsonSettings), expectedStatusCode);
        }

        public virtual async Task<TResource> Create(
            string newEntity,
            HttpStatusCode expectedStatusCode = HttpStatusCode.Created)
        {
            var data = await CreateHelper(newEntity, expectedStatusCode);
            if (string.IsNullOrWhiteSpace(data))
            {
                return null;
            }
            return JsonConvert.DeserializeObject<TResource>(data);
        }

        protected async Task<string> CreateHelper(
            string newEntity,
            HttpStatusCode expectedStatusCode = HttpStatusCode.Created)
        {
            var reqUri = $"{_baseUrl}/api/{_name}";
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(reqUri),
                Method = HttpMethod.Post,
                Content = new StringContent(newEntity),
            };
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            AddAuthTokenHeader(request.Headers, AuthToken);

            var response = await _client.SendAsync(request);
            string data = await response.Content.ReadAsStringAsync();
            Assert.Equal(expectedStatusCode, response.StatusCode);
            if (string.IsNullOrWhiteSpace(data) ||
                (response.StatusCode == HttpStatusCode.BadRequest))
            {
                return null;
            }
            System.Console.WriteLine($"Sites.Create output: {data}");
            return data;
        }

        public async Task Delete(
            TId guid,
            HttpStatusCode expectedStatusCode = HttpStatusCode.NoContent)
        {
            await Delete(guid.ToString(), expectedStatusCode);
        }

        public async Task Delete(
            string guid,
            HttpStatusCode expectedStatusCode = HttpStatusCode.NoContent)
        {
            var reqUri = $"{_baseUrl}/api/{_name}/{guid}";
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(reqUri),
                Method = HttpMethod.Delete,
            };
            //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            AddAuthTokenHeader(request.Headers, AuthToken);

            var response = await _client.SendAsync(request);
            Assert.Equal(expectedStatusCode, response.StatusCode);
            return;
        }


    }
}