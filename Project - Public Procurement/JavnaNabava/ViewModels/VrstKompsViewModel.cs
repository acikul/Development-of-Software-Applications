using System;
using System.Collections.Generic;

namespace JavnaNabava.ViewModels
{
    public class VrstKompsViewModel
    {
        public IEnumerable<VrstKompViewModel> VrsteKompetencija { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
