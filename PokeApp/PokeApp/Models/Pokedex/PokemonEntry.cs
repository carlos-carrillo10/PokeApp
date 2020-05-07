using System;
using System.Collections.Generic;
using System.Text;

namespace PokeApp.Models.Pokedex
{
    public class PokemonEntry
    {
        public int entry_number { get; set; }
        public PokemonSpecies pokemon_species { get; set; }
    }
}
