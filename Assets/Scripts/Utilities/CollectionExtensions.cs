using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class CollectionExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
    {
        return !HasItems<T>(collection);
    }

    /// <summary>
    /// Check if an IEnumerable isn't null and contains items.
    /// </summary>
    /// <typeparam name="T">Type of the checked collection</typeparam>
    /// <param name="collection">The collection to check</param>
    /// <returns>'true' if collection isn't null and has elements, otherwise 'false'</returns>
    public static bool HasItems<T>(this IEnumerable<T> collection)
    {
        if (collection == null) return false;
        if (collection is IReadOnlyCollection<T> roc) return roc.Count > 0;
        if (collection is ICollection<T> col) return col.Count > 0;

        return collection.Any();
    }
}
