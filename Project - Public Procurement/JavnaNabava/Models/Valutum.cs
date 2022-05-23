using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class Valutum
    {
        public Valutum()
        {
            Troskovniks = new HashSet<Troskovnik>();
        }

        public int SifValuta { get; set; }
        public string ImeValute { get; set; }
        public string OznValute { get; set; }

        public virtual ICollection<Troskovnik> Troskovniks { get; set; }
    }
}
