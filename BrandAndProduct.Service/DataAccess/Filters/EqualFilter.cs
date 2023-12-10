namespace BrandAndProduct.Service.DataAccess.Filters;

/// <summary>
///  Filter for equality
/// </summary>
/// <typeparam name="T"></typeparam>
public class EqualFilter<T> : IFilter
{
    /// <summary>
    /// Constructor for EqualFilter
    /// </summary>
    /// <param name="propertyName"></param>
    /// <param name="value"></param>
    public EqualFilter(string propertyName, T value)
    {
        PropertyName = propertyName;
        Value = value;
    }

    /// <summary>
    /// The value to compare against
    /// </summary>
    public T Value { get; }

    /// <summary>
    /// The name of the property to compare
    /// </summary>
    public string PropertyName { get; }
}