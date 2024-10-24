using System.Linq.Expressions;

namespace PMSCRM.Utilities
{
    public static class SortingHelper
    {
        // Takes an IEnumerable<T>, the sortBy string (which is the property name to sort by),
        // and the sortDirection string (either "asc" or "desc").
        // Dynamically creates an expression to order the list by the given property.
        // This makes the utility method more flexible,
        // allowing you to sort by any property in your models.
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
