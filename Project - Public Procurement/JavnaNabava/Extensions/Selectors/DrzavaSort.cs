using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JavnaNabava.Extensions.Selectors
{
    public static class DrzavaSort
    {
        public static IQueryable<Drzava> ApplySort(this IQueryable<Drzava> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<Drzava, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.SifDrzave;
                    break;
                case 2:
                    orderSelector = d => d.NazivDrzava;
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
