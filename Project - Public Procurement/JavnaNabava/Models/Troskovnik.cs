using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class Troskovnik
    {
        public Troskovnik()
        {
            Natjecajs = new HashSet<Natjecaj>();
            PrijavaNaNatjecajs = new HashSet<PrijavaNaNatjecaj>();
            Stavkas = new HashSet<Stavka>();
        }

        public int SifTroskovnik { get; set; }
        public int IspravnoPopunjen { get; set; }
        public int SifValuta { get; set; }

        public virtual Valutum SifValutaNavigation { get; set; }
        public virtual ICollection<Natjecaj> Natjecajs { get; set; }
        public virtual ICollection<PrijavaNaNatjecaj> PrijavaNaNatjecajs { get; set; }
        public virtual ICollection<Stavka> Stavkas { get; set; }
    }
}
