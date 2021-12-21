using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using SuperPanel.App.DistributedServices.Abstract;
using SuperPanel.App.Infrastructure;
using SuperPanel.App.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SuperPanel.App.DistributedServices
{
    public class ExternalContactsApi : IExternalContacts
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AsyncRetryPolicy _retryPolicy;

        public ExternalContactsApi(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _retryPolicy = Policy.Handle<HttpRequestException>()
                .WaitAndRetryAsync(AppSettings.ServiceRetryCount, _ => TimeSpan.FromSeconds(AppSettings.ServiceRetryTimeoutSeconds));
        }

        public async Task<User> GetUserBy(int userId)
        {
            var client = _httpClientFactory.CreateClient(AppSettings.ExternalContactsHttpClient);

            return await _retryPolicy.ExecuteAsync(async () =>
            {
                var result = await client.GetAsync($"v1/contacts/{userId}");

                if (!result.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(result.ReasonPhrase);
                }

                var resultString = await result.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<User>(resultString);
            });
        }

        public async Task<User> GetUserBy(string userEmail)
        {
            var client = _httpClientFactory.CreateClient(AppSettings.ExternalContactsHttpClient);

            return await _retryPolicy.ExecuteAsync(async () =>
            {
                var result = await client.GetAsync($"v1/contacts/{userEmail}");

                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                if (!result.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(result.ReasonPhrase);
                }

                var resultString = await result.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<User>(resultString);
            });
        }
        
        public async Task<User> GDPR(int userId)
        {
            var client = _httpClientFactory.CreateClient(AppSettings.ExternalContactsHttpClient);

            return await _retryPolicy.ExecuteAsync(async () =>
            {
                var result = await client.PutAsync($"v1/contacts/{userId}/gdpr", null);

                if (!result.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(result.ReasonPhrase);
                }

                var resultString = await result.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<User>(resultString);
            });
        }
    }
}
