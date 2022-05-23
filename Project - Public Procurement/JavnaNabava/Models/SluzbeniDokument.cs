using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class SluzbeniDokument
    {
        public int SifDokumenta { get; set; }
        public string KlasaDokumenta { get; set; }
        public string ImeDokumenta { get; set; }
        public int UrudzbeniBroj { get; set; }
        public byte[] TekstDokumenta { get; set; }
        public int SifVrsteDokumenta { get; set; }
        public int SifPrijave { get; set; }

        public virtual PrijavaNaNatjecaj SifPrijaveNavigation { get; set; }
        public virtual VrstaDokumentum SifVrsteDokumentaNavigation { get; set; }
    }
}
