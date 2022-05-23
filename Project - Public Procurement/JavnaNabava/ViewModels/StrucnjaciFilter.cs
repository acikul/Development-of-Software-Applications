using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using JavnaNabava.Models;

namespace JavnaNabava.ViewModels
{
    public class StrucnjaciFilter : IPageFilter
    {
        public int? SifStrucneSpreme { get; set; }
        public string NazivStrucneSpreme { get; set; }
        public int? SifGrada { get; set; }
        public string NazivGrada { get; set; }

        public bool IsEmpty()
        {
            bool active = SifStrucneSpreme.HasValue || SifGrada.HasValue;
            return !active;
        }
        public override string ToString()
        {
            return string.Format("{0}-{1}",
                SifStrucneSpreme,
                SifGrada);
        }

        public static StrucnjaciFilter FromString(string s)
        {
            var filter = new StrucnjaciFilter();
            if (!string.IsNullOrEmpty(s))
            {
                string[] arr = s.Split('-', StringSplitOptions.None);

                if (arr.Length == 2)
                {
                    filter.SifStrucneSpreme = string.IsNullOrWhiteSpace(arr[0]) ? new int?() : int.Parse(arr[0]);
                    filter.SifGrada = string.IsNullOrWhiteSpace(arr[1]) ? new int?() : int.Parse(arr[1]);
                }
            }

            return filter;
        }

        public IQueryable<ViewStrucnjakInfo> Apply(IQueryable<ViewStrucnjakInfo> query)
        {
            if (SifStrucneSpreme.HasValue)
            {
                query = query.Where(d => d.SifStrucneSpreme == SifStrucneSpreme.Value);
            }
            if (SifGrada.HasValue)
            {
                query = query.Where(d => d.SifGrada == SifGrada.Value);
            }
            return query;
        }
    }
}
