using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class NatjecajZakon
    {
        public int SifNatjecaj { get; set; }
        public int SifZakon { get; set; }

        public virtual Natjecaj SifNatjecajNavigation { get; set; }
        public virtual Zakon SifZakonNavigation { get; set; }
    }
}
