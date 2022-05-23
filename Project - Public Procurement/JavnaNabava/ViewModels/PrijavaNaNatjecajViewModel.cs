using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JavnaNabava.ViewModels
{
    public class PrijavaNaNatjecajViewModel
    {
        
        public int SifPrijava { get; set; }
        public string VrstaPonuditelja { get; set; }
        public DateTime DatumPrijave { get; set; }
        public string NazivNatjecaja { get; set; }
        public int SifNatjecaja { get; set; }
        public int SifTroskovnika { get; set; }
        public int SifStatus { get; set; }
        public string Status { get; set; }
        public string Ponuditelj { get; set; }
        public string Konzorcij { get; set; }
        public string OIBPonuditelj { get; set; }
        public string OIBKonzorcij { get; set; }
        public IEnumerable<SluzbeniDokumentViewModel> Dokumenti { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (VrstaPonuditelja == "fizicka osoba")
            {
                if (string.IsNullOrWhiteSpace(Ponuditelj))
                {
                    yield return new ValidationResult("Potrebno odabrati ponuditelja", new[] { nameof(Ponuditelj) });
                }
            }
            else if (VrstaPonuditelja == "konzorcij")
            {
                if (string.IsNullOrWhiteSpace(Konzorcij))
                {
                    yield return new ValidationResult("Potrebno je odabrati konzorcij", new[] { nameof(Konzorcij) });
                }
            }

        }
        public PrijavaNaNatjecajViewModel()
        {
            this.Dokumenti = new List<SluzbeniDokumentViewModel>();
        }
    }
}

