using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class Godina
    {
        public Godina()
        {
            PlanNabaves = new HashSet<PlanNabave>();
        }

        public int SifGodine { get; set; }
        public int VrijednostGodine { get; set; }

        public virtual ICollection<PlanNabave> PlanNabaves { get; set; }
    }
}
