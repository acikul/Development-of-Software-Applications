using System;
using System.Collections.Generic;
using JavnaNabava.Models;

namespace JavnaNabava.ViewModels
{
    public class CpviViewModel
    {
        public IEnumerable<CpvViewModel> Cpvi { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
