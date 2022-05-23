using System.Collections.Generic;
using JavnaNabava.Models;


namespace JavnaNabava.ViewModels
{
    public class StrucnjaciViewModel
    {
        public IEnumerable<ViewStrucnjakInfo> Strucnjaci { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public StrucnjaciFilter Filter { get; set; }
    }
}
