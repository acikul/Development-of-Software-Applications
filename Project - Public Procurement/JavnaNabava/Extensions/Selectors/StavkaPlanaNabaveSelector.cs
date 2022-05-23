using System;
using System.Linq;
using JavnaNabava.Models;

namespace JavnaNabava.Extensions.Selectors
{
    public static class StavkaPlanaNabaveSelector
    {
        public static IQueryable<StavkaPlanaNabave> ApplySort(this IQueryable<StavkaPlanaNabave> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<StavkaPlanaNabave, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.SifStavke;
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