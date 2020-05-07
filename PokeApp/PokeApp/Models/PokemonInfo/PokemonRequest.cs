using System;
using System.Collections.Generic;
using System.Text;

namespace PokeApp.Models.PokemonInfo
{
    public class PokemonRequest
    {
        public IList<Abilities> abilities { get; set; }
        public int base_experience { get; set; }
        public IList<Form> forms { get; set; }
        public IList<GameIndice> game_indices { get; set; }
        public int height { get; set; }
        public IList<HeldItem> held_items { get; set; }
        public int id { get; set; }
        public bool is_default { get; set; }
        public string location_area_encounters { get; set; }
        public IList<Move> moves { get; set; }
        public string name { get; set; }
        public int order { get; set; }
        public Species species { get; set; }
        public Sprites sprites { get; set; }
        public IList<Stat> stats { get; set; }
        public IList<Type> types { get; set; }
        public int weight { get; set; }
    }
}
