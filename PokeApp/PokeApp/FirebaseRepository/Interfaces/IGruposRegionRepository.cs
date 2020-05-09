using PokeApp.FireBaseRepository.Interfaces;
using PokeApp.Models.FirebaseDatabase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PokeApp.FireBaseRepository.Interfaces
{
    public interface IGruposRegionRepository : IBaseRepository<GruposRegion>
    {
        Task<bool> SaveGroupByToken(string GroupToken, string UserId, string Region);
        Task<bool> IsMyGroup(string GroupToken, string UserId, string Region);
        Task<bool> ItExist(string GroupToken, string UserId, string Region);
        Task<bool> IsSameRegion(string GroupToken, string UserId, string Region);

    }
}
