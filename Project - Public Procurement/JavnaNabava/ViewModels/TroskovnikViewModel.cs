using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.ViewModels
{
    public class TroskovnikViewModel
    {
        public int SifTroskovnik { get; set; }
        public int IspravnoPopunjen { get; set; }
        public string ImeValute { get; set; }

        //public string NazNatjecaja { get; set; }


        //public IEnumerable<NatjecajViewModel> Natjecaj { get; set; }

        //public IEnumerable<PrijavaNaNatjecajViewModel> Natjecaj  { get; set; }

        public IEnumerable<StavkaViewModel> Stavke { get; set; }

        public TroskovnikViewModel()
        {
            this.Stavke = new List<StavkaViewModel>();
        }

    }
}
