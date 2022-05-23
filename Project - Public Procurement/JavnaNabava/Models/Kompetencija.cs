using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class Kompetencija
    {
        public int SifKompetencije { get; set; }
        public string DetaljiKompetencije { get; set; }
        public string Oibstrucnjaka { get; set; }
        public int SifVrsteKompetencije { get; set; }

        public virtual Strucnjak OibstrucnjakaNavigation { get; set; }
        public virtual VrstaKompetencije SifVrsteKompetencijeNavigation { get; set; }
    }
}
