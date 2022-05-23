using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class VrijednostiKriterija
    {
        public int TezinskaOcjena { get; set; }
        public int MaksBrojBodova { get; set; }
        public int SifNatjecaja { get; set; }
        public int SifKriterija { get; set; }

        public virtual Kriterij SifKriterijaNavigation { get; set; }
        public virtual Natjecaj SifNatjecajaNavigation { get; set; }
    }
}
