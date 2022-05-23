using System;
using System.Linq;
using JavnaNabava.Models;

namespace JavnaNabava.Extensions.Selectors
{
    public static class SluzbeniDokumentSort
    {
        public static IQueryable<SluzbeniDokument> ApplySort(this IQueryable<SluzbeniDokument> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<SluzbeniDokument, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.SifDokumenta;
                    break;
                case 2:
                    orderSelector = d => d.KlasaDokumenta;
                    break;
                case 3:
                    orderSelector = d => d.ImeDokumenta;
                    break;
                case 4:
                    orderSelector = d => d.UrudzbeniBroj;
                    break;
                case 5:
                    orderSelector = d => d.TekstDokumenta;
                    break;
                case 6:
                    orderSelector = d => d.SifVrsteDokumentaNavigation.NazivVrsteDokumenta;
                    break;
                case 7:
                    orderSelector = d => d.SifPrijaveNavigation.SifPrijava;
                    break;
            }
            if (orderSelector != null)
            {
                query = ascending ?
                       query.OrderBy(orderSelector) :
                       query.OrderByDescending(orderSelector);
            }

            return query;
        }
    }
}

