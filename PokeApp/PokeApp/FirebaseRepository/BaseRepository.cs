using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using PokeApp.FireBaseRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            }
            return _instance;
        }
        #endregion

        public BaseRepository()
        {

        }

        public async Task<IEnumerable<T>> GetData<T>()
        {
            try
            {
                var values = await GetInstance().Child(typeof(T).Name).OnceAsync<T>();
                return values.Select(x => x.Object).ToList();
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        public async Task<bool> SaveData<T>(T value)
        {
            try
            {
                var values = JsonConvert.SerializeObject(value);
                var a = await GetInstance().Child(typeof(T).Name).PostAsync(values);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SaveDataRange<T>(IEnumerable<T> values)
        {
            try
            {
                foreach (var item in values)
                {
                    var val = JsonConvert.SerializeObject(item);
                    var a = await GetInstance().Child(typeof(T).Name).PostAsync(val);
                }            
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Task<T> GetDataById(int id)
        {
            return default;
        }

        public Task<bool> UpdateData<T>(T value)
        {
            return default;
        }

        public Task<bool> UpdateDataRange<T>(IEnumerable<T> values)
        {
            return default;
        }

        public Task<bool> DeleteData(int id)
        {
            return default;
        }

        public async Task<int> GetLastID()
        {
            return default;
        }
    }
}
