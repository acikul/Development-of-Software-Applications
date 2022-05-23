using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Models;

namespace JavnaNabava.Extensions.Selectors
{
    public static class GradSort
    {
        public static IQueryable<Grad> ApplySort(this IQueryable<Grad> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<Grad, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = a => a.NazivGrada;
                    break;
                case 2:
                    orderSelector = a => a.SifDrzava;
                    break;
                case 3:
                    orderSelector = a => a.SifGrada;
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
