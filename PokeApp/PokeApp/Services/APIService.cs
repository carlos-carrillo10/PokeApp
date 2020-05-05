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

        public Task<T> GetAsync<T>(string URL)
        {
            throw new NotImplementedException();
        }

        public Task<T> PostAsync<T>(string URL, object data)
        {
            throw new NotImplementedException();
        }
    }
}
