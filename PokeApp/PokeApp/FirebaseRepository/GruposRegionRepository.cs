using Firebase.Database.Query;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using PokeApp.FireBaseRepository.Interfaces;
using PokeApp.Models.FirebaseDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeApp.FireBaseRepository.Repositories
{
    public class GruposRegionRepository : BaseRepository<GruposRegion>, IGruposRegionRepository
    {
        public async Task<GruposRegion> GetDataById(int id)
        {
            try
            {
                var allValues = await GetData<GruposRegion>();
                return allValues.Where(a => a.GrupoId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
               return default;
            }
        }

        public async Task<bool> UpdateData(GruposRegion value)
        {
            try
            {
                //GetKey 
                var valueToUpdate = (await GetInstance().Child(typeof(GruposRegion).Name)
                    .OnceAsync<GruposRegion>()).Where(x =>x.Object.GrupoId == value.GrupoId && x.Object.UserId == value.UserId).FirstOrDefault();

                var newValue = JsonConvert.SerializeObject(value);
                await GetInstance().Child(typeof(GruposRegion).Name).Child(valueToUpdate.Key).PutAsync(newValue);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateDataRange(IEnumerable<GruposRegion> values)
        {
            try
            {
                foreach (var item in values)
                {
                    //GetKey 
                    var valueToUpdate = (await GetInstance().Child(typeof(GruposRegion).Name)
                        .OnceAsync<GruposRegion>()).Where(x => x.Object.GrupoId == item.GrupoId && x.Object.UserId == item.UserId).FirstOrDefault();

                    var newValue = JsonConvert.SerializeObject(item);
                    await GetInstance().Child(typeof(GruposRegion).Name).Child(valueToUpdate.Key).PutAsync(newValue);
                }
                
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteData(int id)
        {
            try
            {
                //GetKey 
                var valueToDelete = (await GetInstance().Child(typeof(GruposRegion).Name)
                    .OnceAsync<GruposRegion>()).Where(x => x.Object.GrupoId == id).FirstOrDefault();

                await GetInstance().Child(typeof(GruposRegion).Name).Child(valueToDelete.Key).DeleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<int> GetLastID()
        {
            return (await GetData<GruposRegion>()).OrderBy(x =>x.GrupoId).LastOrDefault().GrupoId;
        }
    }
}
