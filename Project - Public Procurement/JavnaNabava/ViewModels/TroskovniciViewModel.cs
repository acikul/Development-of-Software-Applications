using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.ViewModels
{
    public class TroskovniciViewModel
    {
        public IEnumerable<TroskovnikViewModel> Troskovnici { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
