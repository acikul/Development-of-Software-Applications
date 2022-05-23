using System;
using System.Collections.Generic;
using JavnaNabava.Models;

namespace JavnaNabava.ViewModels
{
    public class PrijaveNaNatjecajViewModel
    {
        public IEnumerable<ViewPrijavaNaNatjecajInfo> Prijave { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public PrijavaNaNatjecajFilter Filter { get; set; }
    }
}
