using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.ViewModels
{
    public class OvlasteniciViewModel
    {

        public IEnumerable<OvlastenikViewModel> Ovlastenici { get; set; }
        public PagingInfo PagingInfo { get; set; }

    }
}
