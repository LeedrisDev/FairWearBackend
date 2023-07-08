namespace BrandAndProductDatabase.Service.DataAccess.Filters;

public class EqualFilter<T> : IFilter
{
    public EqualFilter(string propertyName, T value)
    {
        PropertyName = propertyName;
        Value = value;
    }

    public T Value { get; }

    public string PropertyName { get; }
}