using PokeApp.FireBaseRepository.Interfaces;
using PokeApp.Models.FirebaseDatabase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PokeApp.FirebaseRepository.Interfaces
{
    public interface IGrupoPokemonsRepository : IBaseRepository<GrupoPokemons>
    {
        Task<IEnumerable<GrupoPokemons>> GetDataByGrupoId(int GrupoId);
        Task<bool> DeteleDataByGrupoId(int GrupoId);
    }
}
