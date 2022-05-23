using JavnaNabava.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.Extensions.Selectors
{
    public static class StavkaSort
    {

        public static IQueryable<Stavka> ApplySort(this IQueryable<Stavka> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<Stavka, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = m => m.SifStavke;
                    break;
                case 2:
                    orderSelector = m => m.NazivStavke;
                    break;
                case 3:
                    orderSelector = m => m.JedCijena;
                    break;
                case 4:
                    orderSelector = m => m.StopaPdv;
                    break;
                case 5:
                    orderSelector = m => m.Kolicina;
                    break;
                case 6:
                    orderSelector = m => m.UkCijena;
                    break;
                case 7:
                    orderSelector = m => m.NapomenaPonuditelja;
                    break;
                case 8:
                    orderSelector = m => m.JedCijenaSaPdv;
                    break;
                case 9:
                    orderSelector = m => m.UkCijenaSaPdv;
                    break;
                case 10:
                    orderSelector = m => m.IznosPdv;
                    break;
                case 11:
                    orderSelector = m => m.NapomenaNarucitelja;
                    break;
                case 12:
                    orderSelector = m => m.SifTroskovnikNavigation.SifTroskovnik;
                    break;
                case 13:
                    orderSelector = m => m.SifVrstaStavkeNavigation.NazivVrsteStavke;
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
