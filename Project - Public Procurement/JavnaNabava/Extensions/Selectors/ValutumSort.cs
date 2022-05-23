using JavnaNabava.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.Extensions.Selectors
{
    public static class ValutumSort
    {
        public static IQueryable<Valutum> ApplySort(this IQueryable<Valutum> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<Valutum, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = m => m.SifValuta;
                    break;
                case 2:
                    orderSelector = m => m.ImeValute;
                    break;
                case 3:
                    orderSelector = m => m.OznValute;
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
