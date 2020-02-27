using System;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace WebApp
{
    public class RestClient
    {
        private string _apiBasicUri = "";
        public RestClient(string url) { _apiBasicUri = url; }
        public async Task<R> PostAsync<R, T>(string url, T contentValue)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiBasicUri);
                var content = new StringContent(JsonConvert.SerializeObject(contentValue), Encoding.UTF8, "application/json");
                var result = await client.PostAsync(url, content);
                result.EnsureSuccessStatusCode();
                string resultContentString = "";
                if (result.Content != null)
                    resultContentString = await result.Content.ReadAsStringAsync();
                R resultContent = JsonConvert.DeserializeObject<R>(resultContentString);
                return resultContent;
            }
        }
    }
}
