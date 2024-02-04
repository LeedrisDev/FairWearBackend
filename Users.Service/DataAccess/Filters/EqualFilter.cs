namespace Users.Service.DataAccess.Filters;

/// <summary>
/// Represents a filter for equality comparison on a specific property.
/// </summary>
/// <typeparam name="T">The type of the value to compare.</typeparam>
public class EqualFilter<T> : IFilter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EqualFilter{T}"/> class with the specified property name and value.
    /// </summary>
    /// <param name="propertyName">The name of the property to filter.</param>
    /// <param name="value">The value to compare for equality.</param>
    public EqualFilter(string propertyName, T value)
    {
        PropertyName = propertyName;
        Value = value;
    }

    /// <summary>
    /// Gets the value to compare for equality.
    /// </summary>
    public T Value { get; }

    /// <summary>
    /// Gets the name of the property to filter.
    /// </summary>
    public string PropertyName { get; }
}
