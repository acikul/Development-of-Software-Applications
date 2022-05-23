using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class VrstaStavke
    {
        public VrstaStavke()
        {
            Stavkas = new HashSet<Stavka>();
        }

        public int SifVrsteStavke { get; set; }
        public string NazivVrsteStavke { get; set; }

        public virtual ICollection<Stavka> Stavkas { get; set; }
    }
}
