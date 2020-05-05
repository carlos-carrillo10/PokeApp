using System;
using System.Collections.Generic;
using System.Text;

namespace PokeApp.Models
{
    public class RegionRequest
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public IList<Region> results { get; set; }
    }
}
