using System;
namespace JavnaNabava.ViewModels
{
    public class SluzbeniDokumentViewModel
    {
        public int SifDokumenta { get; set; }
        public string KlasaDokumenta { get; set; }
        public string ImeDokumenta { get; set; }
        public int UrudzbeniBroj { get; set; }
        public string VrstaDokumenta { get; set; }
        public int sifVrsteDokumenta { get; set; }
        public int SifPrijave { get; set; }
        public byte[] TekstDokumenta { get; set; }
    }
}
