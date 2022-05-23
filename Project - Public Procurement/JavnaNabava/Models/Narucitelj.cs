using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class Narucitelj
    {
        public Narucitelj()
        {
            KontaktNaruciteljs = new HashSet<KontaktNarucitelj>();
            Natjecajs = new HashSet<Natjecaj>();
            Ovlasteniks = new HashSet<Ovlastenik>();
            PlanNabaves = new HashSet<PlanNabave>();
        }

        public string Oibnarucitelja { get; set; }
        public string NazivNarucitelja { get; set; }
        public string AdresaNarucitelja { get; set; }
        public int PbrNarucitelja { get; set; }
        public string KlasaNarucitelja { get; set; }
        public int SifGrada { get; set; }

        public virtual Grad SifGradaNavigation { get; set; }
        public virtual ICollection<KontaktNarucitelj> KontaktNaruciteljs { get; set; }
        public virtual ICollection<Natjecaj> Natjecajs { get; set; }
        public virtual ICollection<Ovlastenik> Ovlasteniks { get; set; }
        public virtual ICollection<PlanNabave> PlanNabaves { get; set; }
    }
}
