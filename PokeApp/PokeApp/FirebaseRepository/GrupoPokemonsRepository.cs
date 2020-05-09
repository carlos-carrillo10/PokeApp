using Firebase.Database.Query;
using Newtonsoft.Json;
using PokeApp.FirebaseRepository.Interfaces;
using PokeApp.FireBaseRepository.Repositories;
using PokeApp.Models.FirebaseDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeApp.FirebaseRepository
{
    public class GrupoPokemonsRepository : BaseRepository<GrupoPokemons>, IGrupoPokemonsRepository
    {

        public async Task<GrupoPokemons> GetDataById(int id, string UserId, string Region)
        {
            try
            {
                var value = await GetAllData(UserId, Region);
                if (value != null)
                    return value.Where(a => a.Id == id).FirstOrDefault();
                else
                    return default;
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        public async Task<bool> SaveDataRange(IEnumerable<GrupoPokemons> values)
        {
            try
            {
                foreach (var item in values)
                {
                    item.Id = await GetLastID() + 1;
                    var val = JsonConvert.SerializeObject(item);
                    await GetInstance().Child(typeof(GrupoPokemons).Name).PostAsync(val);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateData(GrupoPokemons value)
        {
            try
            {
                //GetKey 
                var valueToUpdate = (await GetInstance().Child(typeof(GrupoPokemons).Name)
                    .OnceAsync<GrupoPokemons>()).Where(x => x.Object.Id == value.Id && x.Object.Pokemon == value.Pokemon
                    && x.Object.GroupId == value.GroupId).FirstOrDefault();

                var newValue = JsonConvert.SerializeObject(value);
                await GetInstance().Child(typeof(GrupoPokemons).Name).Child(valueToUpdate.Key).PutAsync(newValue);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateDataRange(IEnumerable<GrupoPokemons> values)
        {
            try
            {
                foreach (var item in values)
                {
                    //GetKey 
                    var valueToUpdate = (await GetInstance().Child(typeof(GrupoPokemons).Name)
                        .OnceAsync<GrupoPokemons>()).Where(x => x.Object.Id == item.Id && x.Object.Pokemon == item.Pokemon
                    && x.Object.GroupId == item.GroupId).FirstOrDefault();

                    var newValue = JsonConvert.SerializeObject(item);
                    await GetInstance().Child(typeof(GrupoPokemons).Name).Child(valueToUpdate.Key).PutAsync(newValue);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteData(int id, string UserId, string Regio)
        {
            try
            {
                //GetKey 
                var valueToDelete = (await GetInstance().Child(typeof(GrupoPokemons).Name)
                    .OnceAsync<GrupoPokemons>()).Where(x => x.Object.Id == id).FirstOrDefault();

                await GetInstance().Child(typeof(GrupoPokemons).Name).Child(valueToDelete.Key).DeleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<int> GetLastID()
        {
            try
            {
                var value = (await GetInstance().Child(typeof(GrupoPokemons).Name).OnceAsync<GrupoPokemons>()).Select(x =>x.Object);
                if (value != null)
                    return value.OrderBy(x => x.Id).LastOrDefault().Id;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public async Task<bool> DeteleDataByGrupoId(int GrupoId)
        {
            try
            {
                //GetKey 
                var valuesToDelete = (await GetInstance().Child(typeof(GrupoPokemons).Name)
                    .OnceAsync<GrupoPokemons>()).Where(x => x.Object.GroupId == GrupoId).ToList();

                foreach (var item in valuesToDelete)
                {
                    await GetInstance().Child(typeof(GrupoPokemons).Name).Child(item.Key).DeleteAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<GrupoPokemons>> GetDataByGrupoId(int GrupoId)
        {
            try
            {
                //GetKey 
                var values = await GetInstance().Child(typeof(GrupoPokemons).Name).OnceAsync<GrupoPokemons>();
                if (values.Count > 0)
                    return values.Where(x => x.Object.GroupId == GrupoId).ToList().Select(x => x.Object);
                else
                    return default;

            }
            catch (Exception ex)
            {
                return default;
            }
        }

        public async Task<IEnumerable<GrupoPokemons>> GetAllDataById(int id, string UserId, string Region)
        {
            try
            {
                var value = await GetAllData(UserId, Region);
                if (value != null)
                    return value.Where(a => a.Id == id);
                else
                    return new List<GrupoPokemons>();
            }
            catch (Exception ex)
            {
                return new List<GrupoPokemons>();
            }
        }

    }
}
