using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class Natjecaj
    {
        public Natjecaj()
        {
            NatjecajZakons = new HashSet<NatjecajZakon>();
            PrijavaNaNatjecajs = new HashSet<PrijavaNaNatjecaj>();
            VrijednostiKriterijas = new HashSet<VrijednostiKriterija>();
        }

        public int SifNatjecaja { get; set; }
        public string StatusNatjecaja { get; set; }
        public string NazivNatjecaja { get; set; }
        public DateTime DatumObjave { get; set; }
        public int RokPredaje { get; set; }
        public int ElektronickaPredaja { get; set; }
        public int SifTroskovnika { get; set; }
        public string SifCpv { get; set; }
        public int SifStavke { get; set; }
        public string Oibnarucitelja { get; set; }

        public virtual Narucitelj OibnaruciteljaNavigation { get; set; }
        public virtual Cpv SifCpvNavigation { get; set; }
        public virtual StavkaPlanaNabave SifStavkeNavigation { get; set; }
        public virtual Troskovnik SifTroskovnikaNavigation { get; set; }
        public virtual ICollection<NatjecajZakon> NatjecajZakons { get; set; }
        public virtual ICollection<PrijavaNaNatjecaj> PrijavaNaNatjecajs { get; set; }
        public virtual ICollection<VrijednostiKriterija> VrijednostiKriterijas { get; set; }
    }
}
