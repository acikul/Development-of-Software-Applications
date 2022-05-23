using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class StatusPrijave
    {
        public StatusPrijave()
        {
            PrijavaNaNatjecajs = new HashSet<PrijavaNaNatjecaj>();
        }

        public int SifStatusa { get; set; }
        public string NazivStatusa { get; set; }

        public virtual ICollection<PrijavaNaNatjecaj> PrijavaNaNatjecajs { get; set; }
    }
}
