using System;
using System.Linq;
using JavnaNabava.Models;

namespace JavnaNabava.Extensions.Selectors
{
    public static class PlanNabaveSort
    {
        public static IQueryable<PlanNabave> ApplySort(this IQueryable<PlanNabave> query, int sort, bool ascending)
        {
            System.Linq.Expressions.Expression<Func<PlanNabave, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.SifPlanaNabave;
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