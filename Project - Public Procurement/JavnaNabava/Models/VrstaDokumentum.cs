using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class VrstaDokumentum
    {
        public VrstaDokumentum()
        {
            SluzbeniDokuments = new HashSet<SluzbeniDokument>();
        }

        public int SifVrsteDokumenta { get; set; }
        public string NazivVrsteDokumenta { get; set; }

        public virtual ICollection<SluzbeniDokument> SluzbeniDokuments { get; set; }
    }
}
