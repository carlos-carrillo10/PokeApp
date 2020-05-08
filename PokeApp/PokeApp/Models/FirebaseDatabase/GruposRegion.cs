using System;
using System.Collections.Generic;
using System.Text;

namespace PokeApp.Models.FirebaseDatabase
{
    public class GruposRegion
    {
        public int GrupoId { get; set; }
        public string GrupoName { get; set; }
        public string GrupoTipo { get; set; }
        public string PokedexDescription { get; set; }
        public string Image { get; set; }
        public string Region { get; set; }
        public string UserId { get; set; }
    }
}
