using System;
using System.Collections.Generic;
using System.Text;

namespace PokeApp.Models
{
    public class HeldItem
    {
        public Item item { get; set; }
        public IList<VersionDetail> version_details { get; set; }
    }
}
