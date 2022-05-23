using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class Zakon
    {
        public Zakon()
        {
            NatjecajZakons = new HashSet<NatjecajZakon>();
        }

        public int SifZakon { get; set; }
        public string NazivZakona { get; set; }

        public virtual ICollection<NatjecajZakon> NatjecajZakons { get; set; }
    }
}
