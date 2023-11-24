namespace Users.Service.DataAccess.Filters;

public class GenericFilter<TFilter> where TFilter : IFilter
{
    public GenericFilter(IReadOnlyList<TFilter> filters)
    {
        Filters = filters ?? throw new ArgumentNullException(nameof(filters));
    }

    public IReadOnlyList<TFilter> Filters { get; }
}