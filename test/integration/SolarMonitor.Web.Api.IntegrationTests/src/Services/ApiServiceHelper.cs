using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using SolarMonitor.Data.Resources;
using System;
using System.Net.Http.Headers;

namespace SolarMonitor.Api.IntegrationTests
{
    public class ApiServiceHelper
    {
        protected HttpClient _client;
        protected const string _baseUrl = "https://localhost:5001";
        protected TokenProvider TokenProvider { get; private set; }

        /// token used for API calls
        public string AuthToken { get; set; }

        public ApiServiceHelper(HttpClient client, TokenProvider tokenProvider)
        {
            _client = client;
            TokenProvider = tokenProvider;
            AuthToken = TokenProvider.UserToken;
        }


        public async Task<CollectionResource<Sensor>> GetSensors(
            string queryString = "",
            HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
        {
            var reqUri = $"{_baseUrl}/api/sensors{queryString}";
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(reqUri),
                Method = HttpMethod.Get,
            };
            //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            AddAuthTokenHeader(request.Headers, AuthToken);

            var response = await _client.SendAsync(request);
            string data = await response.Content.ReadAsStringAsync();
            Assert.Equal(expectedStatusCode, response.StatusCode);
            return JsonConvert.DeserializeObject<CollectionResource<Sensor>>(data);
        }

        public async Task<Sensor> GetSensor(
            Guid guid,
            HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
        {
            var reqUri = $"{_baseUrl}/api/sensors/{guid}";
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
            return JsonConvert.DeserializeObject<Sensor>(data);
        }



        // Helper functions
        protected void AddAuthorizationHeader(HttpRequestHeaders requestHeaders, string username, string password)
        {
            var credentials = Encoding.UTF8.GetBytes($"{username}:{password}");
            requestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credentials));
        }

        protected void AddAuthTokenHeader(HttpRequestHeaders requestHeaders, string authToken)
        {
            if (authToken != "")
            {
                requestHeaders.Add("Authorization", $"Bearer {authToken}");
            }
        }

    }
}
