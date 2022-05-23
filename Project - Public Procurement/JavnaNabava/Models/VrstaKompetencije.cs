using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class VrstaKompetencije
    {
        public VrstaKompetencije()
        {
            Kompetencijes = new HashSet<Kompetencija>();
        }

        public int SifVrsteKompetencije { get; set; }
        public string NazivVrsteKompetencije { get; set; }

        public virtual ICollection<Kompetencija> Kompetencijes { get; set; }
    }
}
