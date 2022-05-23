using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class PrijavaNaNatjecaj
    {
        public PrijavaNaNatjecaj()
        {
            SluzbeniDokuments = new HashSet<SluzbeniDokument>();
        }

        public int SifPrijava { get; set; }
        public string VrstaPonuditelja { get; set; }
        public DateTime DatumPrijave { get; set; }
        public int SifNatjecaja { get; set; }
        public int SifTroskovnika { get; set; }
        public string Oibponuditelja { get; set; }
        public int SifStatusa { get; set; }
        public string Oibkonzorcija { get; set; }

        public virtual Konzorcij OibkonzorcijaNavigation { get; set; }
        public virtual Ponuditelj OibponuditeljaNavigation { get; set; }
        public virtual Natjecaj SifNatjecajaNavigation { get; set; }
        public virtual StatusPrijave SifStatusaNavigation { get; set; }
        public virtual Troskovnik SifTroskovnikaNavigation { get; set; }
        public virtual ICollection<SluzbeniDokument> SluzbeniDokuments { get; set; }
    }
}
