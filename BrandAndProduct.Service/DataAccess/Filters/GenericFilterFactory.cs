namespace BrandAndProduct.Service.DataAccess.Filters;

/// <summary>
/// GenericFilterFactory
/// </summary>
/// <typeparam name="TFilter"></typeparam>
public class GenericFilterFactory<TFilter> : IFilterFactory<TFilter> where TFilter : IFilter
{
    /// <summary>
    /// CreateFilter
    /// </summary>
    /// <param name="filters"></param>
    /// <returns></returns>
    public GenericFilter<TFilter> CreateFilter(Dictionary<string, string> filters)
    {
        var filterList = new List<TFilter>();
        
        foreach (var pair in filters)
        {
            var filter = (TFilter)(object)new EqualFilter<string>(pair.Key, pair.Value);
            filterList.Add(filter);
        }

        return new GenericFilter<TFilter>(filterList);
    }
}