using System;
using System.Linq;
using JavnaNabava.Models;

namespace JavnaNabava.Extensions.Selectors
{
    public static class CpvSort
    {
        public static IQueryable<Cpv> ApplySort(this IQueryable<Cpv> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<Cpv, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.SifCpv;
                    break;
                case 2:
                    orderSelector = d => d.OpisCpv;
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

