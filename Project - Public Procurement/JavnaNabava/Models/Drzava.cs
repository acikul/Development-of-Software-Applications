using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class Drzava
    {
        public Drzava()
        {
            Grads = new HashSet<Grad>();
            Konzorcijs = new HashSet<Konzorcij>();
        }

        public int SifDrzave { get; set; }
        public string NazivDrzava { get; set; }

        public virtual ICollection<Grad> Grads { get; set; }
        public virtual ICollection<Konzorcij> Konzorcijs { get; set; }
    }
}
