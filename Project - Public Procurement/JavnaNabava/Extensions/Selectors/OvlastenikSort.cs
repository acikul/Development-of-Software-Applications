using JavnaNabava.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.Extensions.Selectors
{
    public static class OvlastenikSort
    {

        public static IQueryable<Ovlastenik> ApplySort(this IQueryable<Ovlastenik> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<Ovlastenik, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.Oibovlastenika;
                    break;
                case 2:
                    orderSelector = d => d.ImeOvlastenika;
                    break;
                case 3:
                    orderSelector = d => d.PrezimeOvlastenika;
                    break;
                case 4:
                    orderSelector = d => d.FunkcijaOvlastenika;
                    break;
                case 5:
                    orderSelector = d => d.Oibnarucitelj;
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
