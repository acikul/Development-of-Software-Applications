using System;
namespace JavnaNabava.ViewModels
{
    public class StavkaPlanaNabaveViewModel
    {
        public int SifStavke { get; set; }

        public string EvidencijskiBroj { get; set; }
        public string PredmetNabave { get; set; }
        public DateTime VrijediOd { get; set; }
        public DateTime VrijediDo { get; set; }
        public string Napomena { get; set; }
        public string VrstaPostupka { get; set; }
        public double VrijednostNabave { get; set; }
        public int SifPlanaNabave { get; set; }
        public int SifStatusa { get; set; }
        public string NazivStatusa { get; set; }
        public string NazivNarucitelja { get; set; }



    }
}
