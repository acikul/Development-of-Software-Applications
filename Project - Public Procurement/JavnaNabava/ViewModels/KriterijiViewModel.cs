using System;
using System.Collections.Generic;

namespace JavnaNabava.ViewModels
{
    public class KriterijiViewModel
    {
        public IEnumerable<KriterijViewModel> Kriteriji { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
