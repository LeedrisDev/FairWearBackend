namespace Users.Service.DataAccess.Filters;

/// <summary>
/// Represents a container for generic filters of type <typeparamref name="TFilter"/>.
/// </summary>
/// <typeparam name="TFilter">The type of filters contained in the generic filter.</typeparam>
public class GenericFilter<TFilter> where TFilter : IFilter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GenericFilter{TFilter}"/> class with the specified filters.
    /// </summary>
    /// <param name="filters">The list of filters to be contained in the generic filter.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="filters"/> is <c>null</c>.</exception>
    public GenericFilter(IReadOnlyList<TFilter> filters)
    {
        Filters = filters ?? throw new ArgumentNullException(nameof(filters));
    }

    /// <summary>
    /// Gets the list of filters contained in the generic filter.
    /// </summary>
    public IReadOnlyList<TFilter> Filters { get; }
}
