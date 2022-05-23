using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class Konzorcij
    {
        public Konzorcij()
        {
            Ponuditeljs = new HashSet<Ponuditelj>();
            PrijavaNaNatjecajs = new HashSet<PrijavaNaNatjecaj>();
        }

        public string Oibkonzorcija { get; set; }
        public string NazivKonzorcija { get; set; }
        public int SifDrzave { get; set; }

        public virtual Drzava SifDrzaveNavigation { get; set; }
        public virtual ICollection<Ponuditelj> Ponuditeljs { get; set; }
        public virtual ICollection<PrijavaNaNatjecaj> PrijavaNaNatjecajs { get; set; }
    }
}
