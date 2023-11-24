namespace Users.Service.DataAccess.Filters;

public interface IFilterFactory<TFilter> where TFilter : IFilter
{
    public GenericFilter<TFilter> CreateFilter(Dictionary<string, string> filters);
}