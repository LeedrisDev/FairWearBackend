namespace Users.Service.DataAccess.Filters;

/// <summary>
/// Represents a factory for creating generic filters of type <typeparamref name="TFilter"/>.
/// </summary>
/// <typeparam name="TFilter">The type of filters created by the factory.</typeparam>
public class GenericFilterFactory<TFilter> : IFilterFactory<TFilter> where TFilter : IFilter
{
    /// <summary>
    /// Creates a generic filter based on the provided dictionary of filters.
    /// </summary>
    /// <param name="filters">The dictionary containing filter property names and values.</param>
    /// <returns>A new instance of <see cref="GenericFilter{TFilter}"/> with filters created from the dictionary.</returns>
    public GenericFilter<TFilter> CreateFilter(Dictionary<string, string> filters)
    {
        var filterList = new List<TFilter>();
        foreach (var pair in filters)
        {
            // Create an EqualFilter for each key-value pair in the dictionary
            var filter = (TFilter)(object)new EqualFilter<string>(pair.Key, pair.Value);
            filterList.Add(filter);
        }

        return new GenericFilter<TFilter>(filterList);
    }
}
