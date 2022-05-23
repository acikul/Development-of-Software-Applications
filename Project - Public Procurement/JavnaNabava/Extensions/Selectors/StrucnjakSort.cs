using JavnaNabava.Models;
using System;
using System.Linq;

namespace JavnaNabava.Extensions.Selectors
{
    public static class StrucnjakSort
    {
        public static IQueryable<ViewStrucnjakInfo> ApplySort(this IQueryable<ViewStrucnjakInfo> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<ViewStrucnjakInfo, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.Oibstrucnjaka;
                    break;
                case 2:
                    orderSelector = d => d.ImeStrucnjaka;
                    break;
                case 3:
                    orderSelector = d => d.PrezimeStrucnjaka;
                    break;
                case 4:
                    orderSelector = d => d.EmailStucnjaka;
                    break;
                case 5:
                    orderSelector = d => d.BrojMobitelaStrucnjaka;
                    break;
                case 6:
                    orderSelector = d => d.NazivPonuditelja;
                    break;
                case 7:
                    orderSelector = d => d.NazivStrucneSpreme;
                    break;
                case 8:
                    orderSelector = d => d.NazivGrada;
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
