using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Models;

namespace JavnaNabava.ViewModels
{
    public class PonuditeljViewModel
    {
        public Ponuditelj ponuditelj { get; set; }

        public List<KontaktPonuditelj> kontakti { get; set; }
        public string prethodniKontroler { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
