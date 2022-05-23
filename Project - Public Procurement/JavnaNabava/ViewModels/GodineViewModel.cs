using System;
using System.Collections.Generic;

namespace JavnaNabava.ViewModels
{
    public class GodineViewModel
    {
        public IEnumerable<GodinaViewModel> Godine { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
