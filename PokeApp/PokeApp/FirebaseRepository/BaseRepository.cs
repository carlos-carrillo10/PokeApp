using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using PokeApp.FireBaseRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PokeApp.FireBaseRepository.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        #region Singleton
        private static FirebaseClient _instance;

        public static FirebaseClient GetInstance()
        {
            if (_instance == null)
            {
                _instance = new FirebaseClient("https://pokeapp-276302.firebaseio.com");
                   // new FirebaseOptions { AuthTokenAsyncFactory = () => Task.FromResult((SecureStorage.GetAsync("Token")).ToString())});
            }
            return _instance;
        }
        #endregion

        public async Task<IEnumerable<T>>GetAllData(string UserId, string Region)
        {
            try
            {
                var values = await GetInstance().Child(typeof(T).Name).OnceAsync<T>();
                if (values.Count > 0)
                    return values.Select(x => x.Object).ToList();
                else
                    return new List<T>();
            }
            catch (Exception ex)
            {
                return new List<T>();
            }
        }

        public async Task<bool> SaveData(T value)
        {
            try
            {
                var values = JsonConvert.SerializeObject(value);
                await GetInstance().Child(typeof(T).Name).PostAsync(values);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

      
        public Task<T> GetDataById(int id, string UserId, string Region)
        {
            return default;
        }

        public Task<bool> UpdateData(T value)
        {
            return default;
        }

        public Task<bool> UpdateDataRange(IEnumerable<T> values)
        {
            return default;
        }

        public Task<bool> DeleteData(int id, string UserId, string Region)
        {
            return default;
        }

        public async Task<int> GetLastID()
        {
            return default;
        }

        public Task<T> GetDataByName(string Name, string UserId, string Region)
        {
            return default;
        }

        public Task<IEnumerable<T>> GetAllDataById(int id, string UserId, string Region)
        {
            return default;
        }

        public Task<IEnumerable<T>> GetAllDataByName(string Name, string UserId)
        {
           return default;
        }

        public virtual Task<bool> SaveDataRange(IEnumerable<T> values)
        {
            throw new NotImplementedException();
        }
    }
}
