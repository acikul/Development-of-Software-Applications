using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class KontaktNarucitelj
    {
        public int SifKontakt { get; set; }
        public string TekstKontakta { get; set; }
        public int SifVrsteKontakta { get; set; }
        public string Oibnarucitelja { get; set; }

        public virtual Narucitelj OibnaruciteljaNavigation { get; set; }
        public virtual VrstaKontaka SifVrsteKontaktaNavigation { get; set; }
    }
}
