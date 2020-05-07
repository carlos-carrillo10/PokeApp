using System;
using System.Collections.Generic;
using System.Text;

namespace PokeApp.Models.Pokedex
{
    public class PokemonSpecies
    {
        public string Image { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public bool IsSelected { get; set; }
    }
}
