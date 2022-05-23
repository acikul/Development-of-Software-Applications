using JavnaNabava.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.Extensions.Selectors
{
    public static class TroskovnikSort
    {
        public static IQueryable<Troskovnik> ApplySort(this IQueryable<Troskovnik> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<Troskovnik, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = m => m.SifTroskovnik;
                    break;
                case 2:
                    orderSelector = m => m.IspravnoPopunjen;
                    break;
                case 3:
                    orderSelector = m => m.SifValutaNavigation.ImeValute;
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
