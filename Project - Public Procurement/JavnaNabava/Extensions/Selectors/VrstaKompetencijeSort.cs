using JavnaNabava.Models;
using System;
using System.Linq;

namespace JavnaNabava.Extensions.Selectors
{
    public static class VrstaKompetencijeSort
    {
        public static IQueryable<VrstaKompetencije> ApplySort(this IQueryable<VrstaKompetencije> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<VrstaKompetencije, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.SifVrsteKompetencije;
                    break;
                case 2:
                    orderSelector = d => d.NazivVrsteKompetencije;
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
