using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.ViewModels
{
    public class StavkeViewModel
    {
        public IEnumerable<StavkaViewModel> Stavke { get; set; }
        public PagingInfo PagingInfo { get; set; }

    }
}
