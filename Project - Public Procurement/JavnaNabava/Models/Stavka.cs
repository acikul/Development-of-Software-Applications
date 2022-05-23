using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class Stavka
    {
        public int SifStavke { get; set; }
        public string NazivStavke { get; set; }
        public double JedCijena { get; set; }
        public double StopaPdv { get; set; }
        public double Kolicina { get; set; }
        public double UkCijena { get; set; }
        public string NapomenaPonuditelja { get; set; }
        public double JedCijenaSaPdv { get; set; }
        public double UkCijenaSaPdv { get; set; }
        public double IznosPdv { get; set; }
        public string NapomenaNarucitelja { get; set; }
        public int SifTroskovnik { get; set; }
        public int SifVrstaStavke { get; set; }

        public virtual Troskovnik SifTroskovnikNavigation { get; set; }
        public virtual VrstaStavke SifVrstaStavkeNavigation { get; set; }
    }
}
