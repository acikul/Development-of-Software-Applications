using System;
using System.Collections.Generic;
using JavnaNabava.Models;

namespace JavnaNabava.ViewModels
{
    public class StavkePlanaNabaveViewModel
    {
        public IEnumerable<StavkaPlanaNabaveViewModel> StavkePlanaNabave { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
