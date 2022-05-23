using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace JavnaNabava.ViewModels
{
    public class PlanNabaveViewModel
    {
        public int SifPlanaNabave { get; set; }
        public int SifGodine{ get; set; }

        public int VrijednostGodine { get; set; }
        public string OibNarucitelja { get; set; }
        public string NazivNarucitelja { get; set; }

        public IEnumerable<StavkaPlanaNabaveViewModel> Stavke { get; set; }

        public PlanNabaveViewModel()
        {
            this.Stavke = new List<StavkaPlanaNabaveViewModel>();
        }







    }
}
