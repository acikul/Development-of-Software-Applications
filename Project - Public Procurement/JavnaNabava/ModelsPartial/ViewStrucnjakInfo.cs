using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;


namespace JavnaNabava.Models
{
    public class ViewStrucnjakInfo
    {
        public string Oibstrucnjaka { get; set; }

        public string ImeStrucnjaka { get; set; }
        public string PrezimeStrucnjaka { get; set; }
        public string EmailStucnjaka { get; set; }
        public string BrojMobitelaStrucnjaka { get; set; }
        public string Oibponuditelja { get; set; }
        public string NazivPonuditelja { get; set; }
        public int SifStrucneSpreme { get; set; }
        public string NazivStrucneSpreme { get; set; }
        public int SifGrada { get; set; }
        public string NazivGrada { get; set; }


        [NotMapped]
        public int Position { get; set; } //Position in the result
        [NotMapped]
        public string Komps { get; set; }
    }
}
