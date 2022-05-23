using System;
using System.Collections.Generic;
using JavnaNabava.Models;

namespace JavnaNabava.ViewModels
{
    public class PlanoviNabaveViewModel
    {
        public IEnumerable<PlanNabaveViewModel> PlanoviNabave { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
