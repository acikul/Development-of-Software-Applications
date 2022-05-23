using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class Kriterij
    {
        public Kriterij()
        {
            VrijednostiKriterijas = new HashSet<VrijednostiKriterija>();
        }

        public int SifKriterija { get; set; }
        public string NazivKriterija { get; set; }

        public virtual ICollection<VrijednostiKriterija> VrijednostiKriterijas { get; set; }
    }
}
