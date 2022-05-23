using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class StavkaPlanaNabave
    {
        public StavkaPlanaNabave()
        {
            Natjecajs = new HashSet<Natjecaj>();
        }

        public int SifStavke { get; set; }
        public string EvidencijskiBroj { get; set; }
        public string PredmetNabave { get; set; }
        public DateTime VrijediOd { get; set; }
        public DateTime VrijediDo { get; set; }
        public string Napomena { get; set; }
        public string VrstaPostupka { get; set; }
        public double VrijednostNabave { get; set; }
        public int SifPlanaNabave { get; set; }
        public int SifStatusa { get; set; }

        public virtual PlanNabave SifPlanaNabaveNavigation { get; set; }
        public virtual StatusStavke SifStatusaNavigation { get; set; }
        public virtual ICollection<Natjecaj> Natjecajs { get; set; }
    }
}
