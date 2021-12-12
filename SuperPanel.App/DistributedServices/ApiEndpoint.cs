using Newtonsoft.Json;
using SuperPanel.App.DistributedServices.Abstract;
using SuperPanel.App.Exceptions;
using SuperPanel.App.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SuperPanel.App.DistributedServices
{
    public class ApiEndpoint : IApiEndpoint
    {
        public string Endpoint { get; set; }

        public T GetData<T>(string url)
        {
            var response = MakeRequest(HttpMethod.Get, url, null);

            if (typeof(T) == typeof(String))
                return (T)(object)response;

            return JsonConvert.DeserializeObject<T>(response);
        }

        public T PostData<T>(string url, string body)
        {
            throw new NotImplementedException();
        }

        public T PutData<T>(string url, string body)
        {
            var response = MakeRequest(HttpMethod.Put, url, body);

            if (typeof(T) == typeof(String))
                return (T)(object)response;

            return JsonConvert.DeserializeObject<T>(response);
        }
        public T DeleteData<T>(string url)
        {
            throw new NotImplementedException();
        }

        #region Private methods...
        private string MakeRequest(HttpMethod method, string requestUrl, object body)
        {
            var url = $"{Endpoint}{requestUrl}";
            int retries = 0;
            var payload = string.Empty;

            do
            {
                // set up request
                var http = (HttpWebRequest)WebRequest.Create(url);
                http.ContentType = "application/json";
                http.Method = method.ToString();
                //http.Timeout = Timeout;
                http.AllowAutoRedirect = false;
                
                try
                {
                    retries++;
                    // for post requests, with a payload, write to body
                    if ((method == HttpMethod.Post || method == HttpMethod.Put) && body != null)
                    {
                        using (var writer = new StreamWriter(http.GetRequestStream()))
                        {
                            payload = JsonConvert.SerializeObject(body);
                            writer.Write(payload);
                            writer.Flush();
                        }
                    }
                    else
                    {
                        http.ContentLength = 0;
                    }

                    using (var response = (HttpWebResponse)http.GetResponse())
                    {
                        var responseString = GetResponse(response);
                        return responseString;
                    }
                }
                catch (WebException ex)
                {
                    if (retries == AppSettings.ServiceRetryCount)
                    {
                        throw new ApiException($"{url}\n{method}\n{ex}");
                    }
                }

            } while (true);
        }

        private string GetResponse(HttpWebResponse response)
        {
            var responseString = string.Empty;

            using (var stream = response.GetResponseStream())
            {
                if (stream != null)
                {
                    using (var sr = new StreamReader(stream))
                    {
                        responseString = sr.ReadToEnd();
                    }
                }
            }

            return responseString;
        }
        #endregion        
    }
}
