using PokeApp.Models.Pokedex;
using PokeApp.Models.PokemonInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokeApp.Models.Regions
{
    public class RegionInfo
    {
        public int id { get; set; }
        public IList<Location> locations { get; set; }
        public MainGeneration main_generation { get; set; }
        public string name { get; set; }
        public IList<Names> names { get; set; }
        public IList<Pokedex> pokedexes { get; set; }
        public IList<VersionGroup> version_groups { get; set; }
    }
}
