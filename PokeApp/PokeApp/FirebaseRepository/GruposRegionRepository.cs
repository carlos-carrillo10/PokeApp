using Firebase.Database.Query;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using PokeApp.FirebaseRepository.Interfaces;
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
        private IGrupoPokemonsRepository _grupoPokemonsRepository;

        public GruposRegionRepository(IGrupoPokemonsRepository grupoPokemonsRepository)
        {
            _grupoPokemonsRepository = grupoPokemonsRepository;
        }
        public async Task<IEnumerable<GruposRegion>> GetAllData(string UserId, string Region)
        {
            try
            {
                var values = await GetInstance().Child(typeof(GruposRegion).Name).OnceAsync<GruposRegion>();
                if (values.Count > 0)
                    return values.Select(x => x.Object).Where(x => x.UserId == UserId && x.Region.Contains(Region)).ToList();
                else
                    return new List<GruposRegion>();
            }
            catch (Exception ex)
            {
                return new List<GruposRegion>();
            }
        }

        public async Task<GruposRegion> GetDataById(int id, string UserId, string Region)
        {
            try
            {
                var value = await GetAllData(UserId, Region);
                if (value != null)
                    return value.Where(a => a.GrupoId == id).FirstOrDefault();
                else
                    return default;
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
                    .OnceAsync<GruposRegion>()).Where(x => x.Object.GrupoId == value.GrupoId && x.Object.UserId == value.UserId).FirstOrDefault();

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

        public async Task<bool> DeleteData(int id, string UserId, string Region)
        {
            try
            {
                //GetKey 
                var valueToDelete = (await GetInstance().Child(typeof(GruposRegion).Name)
                    .OnceAsync<GruposRegion>()).Where(x => x.Object.GrupoId == id && x.Object.UserId == UserId
                    && x.Object.Region.Contains(Region)).FirstOrDefault();

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
            try
            {
                return (await GetInstance().Child(typeof(GruposRegion).Name).OnceAsync<GruposRegion>()).Select(x=>x.Object)
                    .OrderBy(x => x.GrupoId).LastOrDefault().GrupoId;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public async Task<GruposRegion> GetDataByName(string Name, string UserId, string Region)
        {
            return (await GetAllData(UserId, Region)).Where(x => x.Region.Contains(Name)).FirstOrDefault();

        }

        public async Task<IEnumerable<GruposRegion>> GetAllDataByName(string Name, string UserId)
        {
            var values = await GetAllData(UserId, Name);
            return values.Where(x => x.Region.Contains(Name)).ToList();

        }

        public async Task<bool> SaveDataRange(IEnumerable<GruposRegion> values)
        {
            try
            {
                foreach (var item in values)
                {
                    item.GrupoId = await GetLastID() + 1;
                    var val = JsonConvert.SerializeObject(item);
                    await GetInstance().Child(typeof(GruposRegion).Name).PostAsync(val);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SaveGroupByToken(string GroupToken, string UserId, string Region)
        {
            try
            {
                var value = (await GetInstance().Child(typeof(GruposRegion).Name).OnceAsync<GruposRegion>())
                        .Where(x => x.Object.Token == GroupToken).Select(x => x.Object).FirstOrDefault();

                //Create Group group first
                var group = new GruposRegion
                {
                    GrupoId = await GetLastID() + 1,
                    GrupoName = value.GrupoName,
                    GrupoTipo = value.GrupoTipo,
                    PokedexDescription = value.PokedexDescription,
                    Image = value.Image,
                    Region = value.Region,
                    UserId = UserId,
                    Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                    GrupoIdFather = value.GrupoId
                };

                await SaveData(group);

                var pokemons = (await _grupoPokemonsRepository.GetDataByGrupoId(value.GrupoId)).ToList();

                //set new GrupoId from new group
                pokemons.ForEach(x => x.GroupId = group.GrupoId);

                await _grupoPokemonsRepository.SaveDataRange(pokemons);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
           

        }

        public async Task<bool> IsMyGroup(string GroupToken, string UserId, string Region)
        {
            var value = (await GetInstance().Child(typeof(GruposRegion).Name).OnceAsync<GruposRegion>())
                       .Where(x => x.Object.Token == GroupToken && x.Object.UserId == UserId && x.Object.Region.Contains(Region))
                       .Select(x => x.Object).FirstOrDefault();

            if (value != null)
                return true; 
            else
                return false;
        }

        public async Task<bool> ItExist(string GroupToken, string UserId, string Region)
        {
            var value = (await GetInstance().Child(typeof(GruposRegion).Name).OnceAsync<GruposRegion>())
                        .Where(x => x.Object.Token == GroupToken)
                        .Select(x => x.Object).FirstOrDefault();


            if (value != null)
                return true;
            else
                return false;
        }

        public async Task<bool> IsSameRegion(string GroupToken, string UserId, string Region)
        {
            var value = (await GetInstance().Child(typeof(GruposRegion).Name).OnceAsync<GruposRegion>())
                        .Where(x => x.Object.Token == GroupToken && x.Object.Region.Contains(Region))
                        .Select(x => x.Object).FirstOrDefault();


            if (value != null)
                return true;
            else
                return false;
        }
    }
}
