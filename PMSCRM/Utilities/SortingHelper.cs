using System.Linq.Expressions;
using System.Reflection;

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





        //public static IEnumerable<T> SortBy<T>(this IEnumerable<T> source, string sortBy, string sortDirection)
        //    {
        //        if (string.IsNullOrEmpty(sortBy))
        //            return source;

        //        // Retrieve the property by name
        //        var property = typeof(T).GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        //        if (property == null)
        //            return source; // Return unmodified source if the property does not exist

        //        // Check if the property type implements IComparable
        //        if (!typeof(IComparable).IsAssignableFrom(property.PropertyType) && Nullable.GetUnderlyingType(property.PropertyType) == null)
        //            return source;

        //        // Build the expression tree for sorting
        //        var param = Expression.Parameter(typeof(T), "x");
        //        var propertyAccess = Expression.Property(param, property);
        //        var convertedAccess = Expression.Convert(propertyAccess, typeof(object));
        //        var sortExpression = Expression.Lambda<Func<T, object>>(convertedAccess, param);

        //        // Apply ordering based on direction
        //        return sortDirection.ToLower() == "desc"
        //            ? source.AsQueryable().OrderByDescending(sortExpression).ToList()
        //            : source.AsQueryable().OrderBy(sortExpression).ToList();
        //    }
        //}
    


