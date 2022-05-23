using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class StatusStavke
    {
        public StatusStavke()
        {
            StavkaPlanaNabaves = new HashSet<StavkaPlanaNabave>();
        }

        public int SifStatusa { get; set; }
        public string NazivStatusa { get; set; }
        public string OpisStatusa { get; set; }

        public virtual ICollection<StavkaPlanaNabave> StavkaPlanaNabaves { get; set; }
    }
}
