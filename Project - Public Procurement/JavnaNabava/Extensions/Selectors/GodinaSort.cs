using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JavnaNabava.Extensions.Selectors
{
    public static class GodinaSort
    {
        public static IQueryable<Godina> ApplySort(this IQueryable<Godina> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<Godina, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.SifGodine;
                    break;
                case 2:
                    orderSelector = d => d.VrijednostGodine;
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
