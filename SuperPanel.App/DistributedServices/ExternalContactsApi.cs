using Newtonsoft.Json;
using SuperPanel.App.DistributedServices.Abstract;
using SuperPanel.App.Helpers;
using SuperPanel.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SuperPanel.App.DistributedServices
{
    public class ExternalContactsApi : IExternalContacts
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ExternalContactsApi(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<User> GetUserBy(int userId)
        {
            var client = _httpClientFactory.CreateClient(AppSettings.ExternalContactsHttpClient);
            var result = await client.GetAsync($"v1/contacts/{userId}");
            var resultString = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<User>(resultString);
        }

        public async Task<User> GetUserBy(string userEmail)
        {
            var client = _httpClientFactory.CreateClient(AppSettings.ExternalContactsHttpClient);
            var result = await client.GetAsync($"v1/contacts/{userEmail}");
            var resultString = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<User>(resultString);
        }
        
        public async Task<User> GDPR(int userId)
        {
            var client = _httpClientFactory.CreateClient(AppSettings.ExternalContactsHttpClient);
            var result = await client.PutAsync($"v1/contacts/{userId}/gdpr", null);
            var resultString = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<User>(resultString);
        }

        public async Task<User> GDPRDelete(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
