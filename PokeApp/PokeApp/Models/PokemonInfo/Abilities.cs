using System;
using System.Collections.Generic;
using System.Text;

namespace PokeApp.Models.PokemonInfo
{
    public class Abilities
    {
        public Ability ability { get; set; }
        public bool is_hidden { get; set; }
        public int slot { get; set; }
    }
}
