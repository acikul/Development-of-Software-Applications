using JavnaNabava.Models;
using System.Collections.Generic;


namespace JavnaNabava.ViewModels
{
    public class KompetencijeViewModel
    {
        public IEnumerable<KompetencijaViewModel> Kompetencije { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
