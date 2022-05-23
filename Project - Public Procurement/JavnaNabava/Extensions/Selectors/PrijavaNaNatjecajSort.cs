using System;
using System.Linq;
using JavnaNabava.Models;

namespace JavnaNabava.Extensions.Selectors
{
    public static class PrijavaNaNatjecajSort
    {
        public static IQueryable<ViewPrijavaNaNatjecajInfo> ApplySort(this IQueryable<ViewPrijavaNaNatjecajInfo> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<ViewPrijavaNaNatjecajInfo, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.SifPrijava;
                    break;
                case 2:
                    orderSelector = d => d.VrstaPonuditelja;
                    break;
                case 3:
                    orderSelector = d => d.DatumPrijave;
                    break;
                case 4:
                    orderSelector = d => d.NazivStatusa;
                    break;
                case 5:
                    orderSelector = d => d.NazivPonuditelj;
                    break;
                case 6:
                    orderSelector = d => d.NazivNatjecaja;
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
