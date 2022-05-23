using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace JavnaNabava.ViewModels
{
    public class StrucnjakViewModel
    {
        [Display(Name = "OIB")]
        [Required(ErrorMessage = "Potrebno je upisati OIB")]
        [StringLength(11)]
        public string Oibstrucnjaka { get; set; }

        [Display(Name = "Ime")]
        [Required(ErrorMessage = "Potrebno je upisati ime")]
        public string ImeStrucnjaka { get; set; }

        [Required(ErrorMessage = "Potrebno je unijeti prezime stručnjaka")]
        [Display(Name = "Prezime")]
        public string PrezimeStrucnjaka { get; set; }

        
        [Display(Name = "e-mail")]
        [Required(ErrorMessage = "Potrebno je unijeti email stručnjaka")]
        public string EmailStrucnjaka { get; set; }

        [Display(Name = "Br. mobitela")]
        [Required(ErrorMessage = "Potrebno je unijeti broj mobitela stručnjaka")]
        public string BrojMobitelaStrucnjaka { get; set; }

        [Display(Name = "OIB poslodavca")]
        [Required(ErrorMessage = "Potrebno je unijeti OIB poslodavca/ponuditelja")]
        public string Oibponuditelja { get; set; }
        public string NazivPonuditelja { get; set; }

        [Display(Name = "Stručna sprema")]
        public int SifStrucneSpreme { get; set; }
        public string NazivStrucneSpreme { get; set; }

        [Display(Name = "Grad")]
        public int SifGrada { get; set; }
        public string NazivGrada { get; set; }

        
        public IEnumerable<KompetencijaViewModel> Kompetencije { get; set; }

        public StrucnjakViewModel()
        {
            this.Kompetencije = new List<KompetencijaViewModel>();
        }
    }
}
