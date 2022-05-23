using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#nullable disable

namespace JavnaNabava.Models
{
    public partial class KontaktPonuditelj
    {
        public int SifKontakt { get; set; }

        [Display(Name = "Kontakt")]
        [Required (ErrorMessage="Unesite sadržaj kontakta")]
        public string TekstKontakta { get; set; }

        [Display(Name = "OIB ponuditelja")]
        [Required (ErrorMessage = "Unesite sadržaj kontakta")]
        [RegularExpression(@"\d{11}",
         ErrorMessage = "OIB mora imati 11 brojeva")]
        public string Oibponuditelja { get; set; }

        [Display(Name = "SifVrsteKontakta")]
        [Required (ErrorMessage = "Unesite sadržaj kontakta")]
        public int SifVrsteKontakta { get; set; }

        public virtual Ponuditelj OibponuditeljaNavigation { get; set; }
        public virtual VrstaKontaka SifVrsteKontaktaNavigation { get; set; }
    }
}
