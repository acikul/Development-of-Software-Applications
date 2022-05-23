using System;
using System.Collections.Generic;

namespace JavnaNabava.ViewModels
{
    public class DrzaveViewModel
    {
        public IEnumerable<DrzavaViewModel> Drzave { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
