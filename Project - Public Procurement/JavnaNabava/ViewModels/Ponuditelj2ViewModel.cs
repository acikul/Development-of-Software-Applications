using JavnaNabava.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.ViewModels
{
    public class Ponuditelj2ViewModel
    {
        public string Oibponuditelja { get; set; }
        public string AdresaPonuditelja { get; set; }
        public string KlasaPonuditelja { get; set; }
        public int PbrPonuditelja { get; set; }
        public string NazivPonuditelja { get; set; }
        public int SifGrada { get; set; }
        public string NazivGrada { get; set; }

        public string Oibkonzorcija { get; set; }

        public virtual IEnumerable<KontaktViewModel> kontakti { get; set; }
        public PagingInfo PagingInfo { get; set; }


    }
}
