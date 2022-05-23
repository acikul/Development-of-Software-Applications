using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Models;

namespace JavnaNabava.Extensions.Selectors
{
    public static class KontaktPonuditeljSort
    {
        public static IQueryable<KontaktPonuditelj> ApplySort(this IQueryable<KontaktPonuditelj> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<KontaktPonuditelj, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = a => a.OibponuditeljaNavigation.NazivPonuditelja;
                    break;
                case 2:
                    orderSelector = a => a.TekstKontakta;
                    break;
                case 3:
                    orderSelector = a => a.SifVrsteKontaktaNavigation.NazivVrsteKontakta;
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
