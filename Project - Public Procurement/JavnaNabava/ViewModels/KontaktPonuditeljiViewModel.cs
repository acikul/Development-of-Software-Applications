using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Models;

namespace JavnaNabava.ViewModels
{
    public class KontaktPonuditeljiViewModel
    {
        public IEnumerable<KontaktPonuditelj> data { get; set; }

        public PagingInfo PagingInfo { get; set; }

    }
}
