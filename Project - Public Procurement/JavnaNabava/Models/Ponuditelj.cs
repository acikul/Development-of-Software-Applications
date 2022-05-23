using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


#nullable disable

namespace JavnaNabava.Models
{
    public partial class Ponuditelj
    {
        public Ponuditelj()
        {
            KontaktPonuditeljs = new HashSet<KontaktPonuditelj>();
            PrijavaNaNatjecajs = new HashSet<PrijavaNaNatjecaj>();
            Strucnjaks = new HashSet<Strucnjak>();
        }

        [Display(Name = "OIB ponuditelja")]
        [Required(ErrorMessage = "Unesite sadržaj kontakta")]
        [RegularExpression(@"\d{11}",
         ErrorMessage = "OIB mora imati 11 brojeva")]
        public string Oibponuditelja { get; set; }

        [Required(ErrorMessage = "Unesite naziv ponuditelja")]

        [Display(Name = "Ponuditelj")]
        public string NazivPonuditelja { get; set; }

        [Required(ErrorMessage = "Unesite adresu ponuditelja")]

        [Display(Name = "Adresa ponuditelja")]
        public string AdresaPonuditelja { get; set; }

        [Required(ErrorMessage = "Unesite poštanski broj ponuditelja")]

        [Display(Name = "Poštanski broj ponuditelja")]
        public int PbrPonuditelja { get; set; }

        [Required(ErrorMessage = "Unesite klasu ponuditelja")]

        [Display(Name = "Klasa ponuditelja")]
        public string KlasaPonuditelja { get; set; }


        [Required(ErrorMessage = "")]
        public int SifGrada { get; set; }

        [Display(Name = "OIB konzorcija")]
        public string Oibkonzorcija { get; set; }

        public virtual Konzorcij OibkonzorcijaNavigation { get; set; }
        public virtual Grad SifGradaNavigation { get; set; }
        public virtual ICollection<KontaktPonuditelj> KontaktPonuditeljs { get; set; }
        public virtual ICollection<PrijavaNaNatjecaj> PrijavaNaNatjecajs { get; set; }
        public virtual ICollection<Strucnjak> Strucnjaks { get; set; }
    }
}
