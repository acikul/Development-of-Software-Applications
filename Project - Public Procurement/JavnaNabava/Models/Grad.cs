using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class Grad
    {
        public Grad()
        {
            Naruciteljs = new HashSet<Narucitelj>();
            Ponuditeljs = new HashSet<Ponuditelj>();
            Strucnjaks = new HashSet<Strucnjak>();
        }

        public int SifGrada { get; set; }
        public string NazivGrada { get; set; }
        public int SifDrzava { get; set; }

        public virtual Drzava SifDrzavaNavigation { get; set; }
        public virtual ICollection<Narucitelj> Naruciteljs { get; set; }
        public virtual ICollection<Ponuditelj> Ponuditeljs { get; set; }
        public virtual ICollection<Strucnjak> Strucnjaks { get; set; }
    }
}
