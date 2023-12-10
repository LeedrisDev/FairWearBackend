namespace BrandAndProduct.Service.DataAccess.Filters;

/// <summary>
/// Interface for FilterFactory
/// </summary>
/// <typeparam name="TFilter"></typeparam>
public interface IFilterFactory<TFilter> where TFilter : IFilter
{
    /// <summary>
    /// Creates a generic filter
    /// </summary>
    /// <param name="filters"></param>
    /// <returns></returns>
    public GenericFilter<TFilter> CreateFilter(Dictionary<string, string> filters);
}