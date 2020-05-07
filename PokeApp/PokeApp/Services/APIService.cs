using Newtonsoft.Json;
using PokeApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PokeApp.Services
{
    public class APIService : IAPIService
    {
        #region Properties
        private readonly HttpClient _httpClient;
        #endregion

        public APIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(Constants.BaseAddress);
        }

        public async Task<T> GetAsync<T>(string URL)
        {
            var response = await _httpClient.GetAsync(URL);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(data);
            }
            else
            {
                return default(T);
            }
        }

        public async Task<T> PostAsync<T>(string URL, object data)
        {
            var jsonToSend = JsonConvert.SerializeObject(data, Formatting.None);
            var body = new StringContent(jsonToSend, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(URL.ToString(), body);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(result);
            }
            else
            {
                return default(T);
            }
        }
    }
}
