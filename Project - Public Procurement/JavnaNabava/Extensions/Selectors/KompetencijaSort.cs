using JavnaNabava.Models;
using System;
using System.Linq;

namespace JavnaNabava.Extensions.Selectors
{
    public static class KompetencijaSort
    {
        public static IQueryable<Kompetencija> ApplySort(this IQueryable<Kompetencija> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<Kompetencija, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.SifKompetencije;
                    break;
                case 2:
                    orderSelector = d => d.DetaljiKompetencije;
                    break;
                case 3:
                    orderSelector = d => d.Oibstrucnjaka;
                    break;
                case 4:
                    orderSelector = d => d.SifVrsteKompetencije;
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
