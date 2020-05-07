using PokeApp.Models.Regions;
using System;
using System.Collections.Generic;
using PokeApp.Models.PokemonInfo;
using System.Text;

namespace PokeApp.Models.Pokedex
{
    public class PokedexRequest
    {
        public IList<Description> descriptions { get; set; }
        public int id { get; set; }
        public bool is_main_series { get; set; }
        public string name { get; set; }
        public IList<Names> names { get; set; }
        public IList<PokemonEntry> pokemon_entries { get; set; }
        public Region region { get; set; }
        public IList<VersionGroup> version_groups { get; set; }
    }
}
