using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.ViewModels
{
    public class GradoviViewModel
    {
        public IEnumerable<GradViewModel> Gradovi { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
