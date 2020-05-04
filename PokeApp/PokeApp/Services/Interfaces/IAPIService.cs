using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PokeApp.Services.Interfaces
{
    public interface IAPIService
    {
        Task<T> GetAsync<T>(string URL);
        Task<T> PostAsync<T>(string URL, object data);
    }
}
