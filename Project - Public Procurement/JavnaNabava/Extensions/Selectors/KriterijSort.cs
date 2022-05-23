using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JavnaNabava.Extensions.Selectors
{
    public static class KriterijSort
    {
        public static IQueryable<Kriterij> ApplySort(this IQueryable<Kriterij> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<Kriterij, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.SifKriterija;
                    break;
                case 2:
                    orderSelector = d => d.NazivKriterija;
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
