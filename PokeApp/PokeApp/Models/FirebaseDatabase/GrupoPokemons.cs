using System;
using System.Collections.Generic;
using System.Text;

namespace PokeApp.Models.FirebaseDatabase
{
    public class GrupoPokemons
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public int GroupId { get; set; }
        public string Pokemon { get; set; }
    }
}
