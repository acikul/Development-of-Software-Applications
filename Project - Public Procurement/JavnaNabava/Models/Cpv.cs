using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class Cpv
    {
        public Cpv()
        {
            Natjecajs = new HashSet<Natjecaj>();
        }

        public string SifCpv { get; set; }
        public string OpisCpv { get; set; }

        public virtual ICollection<Natjecaj> Natjecajs { get; set; }
    }
}
