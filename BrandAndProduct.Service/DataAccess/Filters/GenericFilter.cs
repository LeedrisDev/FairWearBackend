namespace BrandAndProduct.Service.DataAccess.Filters;

/// <summary>
///  GenericFilter
/// </summary>
/// <typeparam name="TFilter"></typeparam>
public class GenericFilter<TFilter> where TFilter : IFilter
{
    /// <summary>
    /// Constructor for GenericFilter
    /// </summary>
    /// <param name="filters"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public GenericFilter(IReadOnlyList<TFilter> filters)
    {
        Filters = filters ?? throw new ArgumentNullException(nameof(filters));
    }

    /// <summary>
    /// The filters to apply
    /// </summary>
    public IReadOnlyList<TFilter> Filters { get; }
}