namespace BrandAndProduct.Service.DataAccess.Filters;

public class GenericFilterFactory<TFilter> : IFilterFactory<TFilter> where TFilter : IFilter
{
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