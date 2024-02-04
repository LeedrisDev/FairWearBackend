namespace Users.Service.DataAccess.Filters;

/// <summary>
/// Represents a factory for creating generic filters based on a dictionary of filters.
/// </summary>
/// <typeparam name="TFilter">The type of filter to create.</typeparam>
public interface IFilterFactory<TFilter> where TFilter : IFilter
{
    /// <summary>
    /// Creates a generic filter based on a dictionary of filters.
    /// </summary>
    /// <param name="filters">A dictionary of filters where the key is the property name and the value is the filter value.</param>
    /// <returns>A generic filter containing a list of filters of type <typeparamref name="TFilter"/>.</returns>
    GenericFilter<TFilter> CreateFilter(Dictionary<string, string> filters);
}
