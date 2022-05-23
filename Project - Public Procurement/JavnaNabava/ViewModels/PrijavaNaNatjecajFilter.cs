using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using JavnaNabava.Models;

namespace JavnaNabava.ViewModels
{
    public class PrijavaNaNatjecajFilter : IPageFilter
    {
        public string VrstaPonuditelja { get; set; }
        public string OIBPonuditelj { get; set; }
        public string NazivPonuditelj { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DatumOd { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DatumDo { get; set; }
        public int? sifNatjecaj { get; set; }
        public string NazivNatjecaja { get; set; }

        public bool IsEmpty()
        {
            bool active = VrstaPonuditelja != null
                          || DatumOd.HasValue
                          || DatumDo.HasValue
                          || sifNatjecaj.HasValue;
            return !active;
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}-{2}-{3}",
                VrstaPonuditelja,
                DatumOd?.ToString("dd.MM.yyyy"),
                DatumDo?.ToString("dd.MM.yyyy"),
                sifNatjecaj);
        }

        public static PrijavaNaNatjecajFilter FromString(string s)
        {
            var filter = new PrijavaNaNatjecajFilter();
            if (!string.IsNullOrEmpty(s))
            {
                string[] arr = s.Split('-', StringSplitOptions.None);

                if (arr.Length == 4)
                {
                    filter.VrstaPonuditelja = string.IsNullOrWhiteSpace(arr[0]) ? null : arr[0];
                    filter.DatumOd = string.IsNullOrWhiteSpace(arr[1]) ? new DateTime?() : DateTime.ParseExact(arr[1], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    filter.DatumDo = string.IsNullOrWhiteSpace(arr[2]) ? new DateTime?() : DateTime.ParseExact(arr[2], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    filter.sifNatjecaj = string.IsNullOrWhiteSpace(arr[3]) ? new int?() : int.Parse(arr[3]);
                }
            }

            return filter;
        }

        public IQueryable<ViewPrijavaNaNatjecajInfo> Apply(IQueryable<ViewPrijavaNaNatjecajInfo> query)
        {
            if (VrstaPonuditelja != null)
            {
                query = query.Where(d => d.VrstaPonuditelja == VrstaPonuditelja);
            }
            if (DatumOd.HasValue)
            {
                query = query.Where(d => d.DatumPrijave >= DatumOd.Value);
            }
            if (DatumDo.HasValue)
            {
                query = query.Where(d => d.DatumPrijave <= DatumDo.Value);
            }
            if (sifNatjecaj.HasValue)
            {
                query = query.Where(d => d.SifNatjecaja >= sifNatjecaj.Value);
            }
            
            return query;
        }
    }
}
