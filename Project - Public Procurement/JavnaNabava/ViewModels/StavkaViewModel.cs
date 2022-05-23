using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.ViewModels
{
    public class StavkaViewModel
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
        public string NazivVrsteStavke { get; set; }

    }
}
