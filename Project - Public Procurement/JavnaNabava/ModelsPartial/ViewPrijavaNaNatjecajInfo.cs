using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JavnaNabava.Util;

namespace JavnaNabava.Models
{
    public class ViewPrijavaNaNatjecajInfo
    {
        public int SifPrijava { get; set; }
        public string VrstaPonuditelja { get; set; }

        [Display(Name = "Datum prijave")]
        [ExcelFormat("dd.mm.yyyy")]
        public DateTime DatumPrijave { get; set; }

        public int SifNatjecaja { get; set; }
        public string NazivNatjecaja { get; set; }
        public int SifTroskovnika { get; set; }
        public string NazivStatusa { get; set; }
        public string NazivPonuditelj { get; set; }
        public string OIBPonuditelj { get; set; }


        [NotMapped]
        public int Position { get; set; } //Position in the result
        

    }
}