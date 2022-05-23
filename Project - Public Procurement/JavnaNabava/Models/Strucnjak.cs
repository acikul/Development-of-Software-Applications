using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class Strucnjak
    {
        public Strucnjak()
        {
            Kompetencijes = new HashSet<Kompetencija>();
        }

        public string Oibstrucnjaka { get; set; }
        public string ImeStrucnjaka { get; set; }
        public string PrezimeStrucnjaka { get; set; }
        public string EmailStucnjaka { get; set; }
        public string BrojMobitelaStrucnjaka { get; set; }
        public string Oibponuditelja { get; set; }
        public int SifStrucneSpreme { get; set; }
        public int SifGrada { get; set; }

        public virtual Ponuditelj OibponuditeljaNavigation { get; set; }
        public virtual Grad SifGradaNavigation { get; set; }
        public virtual StrucnaSprema SifStrucneSpremeNavigation { get; set; }
        public virtual ICollection<Kompetencija> Kompetencijes { get; set; }
    }
}
