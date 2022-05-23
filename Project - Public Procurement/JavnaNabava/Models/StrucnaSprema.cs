using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class StrucnaSprema
    {
        public StrucnaSprema()
        {
            Strucnjaks = new HashSet<Strucnjak>();
        }

        public int SifStrucneSpreme { get; set; }
        public string NazivStrucneSpreme { get; set; }

        public virtual ICollection<Strucnjak> Strucnjaks { get; set; }
    }
}
