using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class PlanNabave
    {
        public PlanNabave()
        {
            StavkaPlanaNabaves = new HashSet<StavkaPlanaNabave>();
        }

        public int SifPlanaNabave { get; set; }
        public int SifGodine { get; set; }
        public string Oibnarucitelja { get; set; }

        public virtual Narucitelj OibnaruciteljaNavigation { get; set; }
        public virtual Godina SifGodineNavigation { get; set; }
        public virtual ICollection<StavkaPlanaNabave> StavkaPlanaNabaves { get; set; }
    }
}
