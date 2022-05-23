using System;
using System.Collections.Generic;
using JavnaNabava.Models;

namespace JavnaNabava.ViewModels
{
    public class SluzbeniDokumentiViewModel
    {
        public IEnumerable<SluzbeniDokumentViewModel> SluzbeniDokumenti { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
