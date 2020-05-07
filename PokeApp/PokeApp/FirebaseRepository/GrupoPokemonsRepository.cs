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
        public async Task<GrupoPokemons> GetDataById(int id)
        {
            try
            {
                var allValues = await GetData<GrupoPokemons>();
                return allValues.Where(a => a.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return default;
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

        public async Task<bool> DeleteData(int id)
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
            return (await GetData<GrupoPokemons>()).OrderBy(x => x.Id).LastOrDefault().Id;
        }

    }
}
