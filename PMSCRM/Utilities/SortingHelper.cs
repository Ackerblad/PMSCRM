using System.Linq.Expressions;
using System.Reflection;

namespace PMSCRM.Utilities
{
    public static class SortingHelper
    {
        public static IEnumerable<T> SortBy<T>(this IEnumerable<T> source, string sortBy, string sortDirection)
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                return source;
            }

            var param = Expression.Parameter(typeof(T), "x");
            var sortExpression = Expression.Lambda<Func<T, object>>(
                Expression.Convert(Expression.Property(param, sortBy), typeof(object)), param);

            return sortDirection == "desc"
                ? source.AsQueryable().OrderByDescending(sortExpression)
                : source.AsQueryable().OrderBy(sortExpression);
        }
    }
}