using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class VrstaKontaka
    {
        public VrstaKontaka()
        {
            KontaktNaruciteljs = new HashSet<KontaktNarucitelj>();
            KontaktPonuditeljs = new HashSet<KontaktPonuditelj>();
        }

        public int SifVrsteKontakta { get; set; }
        public string NazivVrsteKontakta { get; set; }

        public virtual ICollection<KontaktNarucitelj> KontaktNaruciteljs { get; set; }
        public virtual ICollection<KontaktPonuditelj> KontaktPonuditeljs { get; set; }
    }
}
