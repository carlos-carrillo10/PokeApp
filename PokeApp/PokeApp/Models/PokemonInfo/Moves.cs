using System;
using System.Collections.Generic;
using System.Text;

namespace PokeApp.Models.PokemonInfo
{
    public class Moves
    {
        public Move move { get; set; }
        public IList<VersionGroupDetail> version_group_details { get; set; }
    }
}
