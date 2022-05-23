using JavnaNabava.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.ViewModels
{
    public class ValutumiViewModel
    {
        public IEnumerable<ValutumViewModel> Valutumi { get; set; }
        public PagingInfo PagingInfo { get; set; }

    }
}